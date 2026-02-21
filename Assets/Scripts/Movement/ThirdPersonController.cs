using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Controls";
    [SerializeField] private string moveActionName = "Move";
    [SerializeField] private string lookActionName = "Look";
    [SerializeField] private string flyActionName = "Fly";

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float walkAcceleration = 8f;
    [SerializeField] private float walkDeceleration = 30f;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Flight")]
    [SerializeField] private bool canFly = false;
    [SerializeField] private float flySpeed = 7f;
    [SerializeField] private float flyAcceleration = 0.5f;
    [SerializeField] private float flyDeceleration = 30f;
    [SerializeField] private float heightChangeSpeed = 11f;
    [SerializeField] private float gravity = -10f;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationController animationController;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip takeOff_SFX;
    [SerializeField] private AudioClip landing_SFX;

    private CharacterController m_controller;

    private InputAction m_moveAction;
    private InputAction m_flyAction;

    private Vector2 m_moveValue;
    private float m_currentSpeed;
    private float m_currentVerticalSpeed;

    private float m_flyValue;
    private float m_animSpeed;

    [SerializeField] private PlayerState m_playerState = PlayerState.Idle;

    public void OnEnable() => inputActions.FindActionMap(actionMapName).Enable();
    public void OnDisable() => inputActions.FindActionMap(actionMapName).Disable();

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();


        var map = inputActions.FindActionMap(actionMapName, true);
        m_moveAction = map.FindAction(moveActionName, true);
        m_flyAction = map.FindAction(flyActionName, true);
    }

    private void Update()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>();
        m_flyValue = m_flyAction.ReadValue<float>();

        Move();
        if(canFly)
        {
            Fly();
        }
        ApplyGravity();
        UpdateFlight();

        UpdateAnimations(m_playerState, m_moveValue);

    }

    private Vector3 GetMoveDirWorld()
    {
        Transform cam = cameraTransform;

        Vector3 forward = (cam != null) ? cam.forward : transform.forward;
        Vector3 right = (cam != null) ? cam.right : transform.right;

        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        right = Vector3.ProjectOnPlane(right, Vector3.up).normalized;

        return right * m_moveValue.x + forward * m_moveValue.y;
    }

    private void Move()
    {
        Vector3 dir = GetMoveDirWorld();
        bool hasInput = dir.sqrMagnitude > 0.0001f;

        float maxSpeed = (m_playerState == PlayerState.Fly) ? flySpeed : walkSpeed;
        float accel = (m_playerState == PlayerState.Fly) ? flyAcceleration : walkAcceleration;
        float decel = (m_playerState == PlayerState.Fly) ? flyDeceleration : walkDeceleration;

        float rate = hasInput ? accel : decel;
        float targetSpeed = hasInput ? maxSpeed : 0f;
        m_currentSpeed = Mathf.MoveTowards(m_currentSpeed, targetSpeed, rate * Time.deltaTime);

        Vector3 moveDir = hasInput ? dir.normalized : Vector3.zero;

        if (hasInput)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * 360f * Time.deltaTime);
            if (m_controller.isGrounded && m_playerState == PlayerState.Idle)
                m_playerState = PlayerState.Walk;
        }
        else if (m_playerState == PlayerState.Walk && m_currentSpeed <= 0.05f)
        {
            m_playerState = PlayerState.Idle;
        }

        m_controller.Move(moveDir * m_currentSpeed * Time.deltaTime);

    }

    private void Fly()
    {
        bool hasFlyInput = Mathf.Abs(m_flyValue) > 0.001f;

        if (m_playerState != PlayerState.Fly && m_flyValue >= 0.001f)
        {
            audioSource.PlayOneShot(takeOff_SFX);
            m_playerState = PlayerState.Fly;
        }

        if (m_playerState != PlayerState.Fly)
            return;

        float targetVSpeed = hasFlyInput ? (m_flyValue * heightChangeSpeed) : 0f;

        // accel se input, decel se rilasci
        float rate = hasFlyInput ? flyAcceleration : flyDeceleration;

        m_currentVerticalSpeed = Mathf.MoveTowards(
            m_currentVerticalSpeed,
            targetVSpeed,
            rate * Time.deltaTime
        );

        m_controller.Move(Vector3.up * m_currentVerticalSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!m_controller.isGrounded && m_playerState != PlayerState.Fly && m_playerState != PlayerState.Climb)
            m_controller.Move(Vector3.up * gravity * Time.deltaTime);
    }

    private void UpdateFlight()
    {
        if (!m_controller.isGrounded && (m_playerState == PlayerState.Walk || m_playerState == PlayerState.Idle))
            m_playerState = PlayerState.FreeFall;

        if (m_controller.isGrounded && (m_playerState == PlayerState.FreeFall || m_playerState == PlayerState.Fly))
        {
            m_playerState = PlayerState.Idle;
            m_currentVerticalSpeed = 0f;
            audioSource.PlayOneShot(landing_SFX);
        } 
    }

    private void UpdateAnimations(PlayerState playerState, Vector2 moveValue)
    {
        animationController.UpdateAnimations(playerState, moveValue);
    }
}