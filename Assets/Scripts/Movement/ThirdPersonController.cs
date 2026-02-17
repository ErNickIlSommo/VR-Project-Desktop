using Unity.VisualScripting;
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

    //[Header("Status")]
    //[SerializeField] private PlayerStatus status;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turnSpeed = 12f;

    [Header("Flight")]
    [SerializeField] private float flySpeed = 15f;
    [SerializeField] private bool isFlying = false;
    [SerializeField] private float maxIncline = 5f;
    [SerializeField] private float minIncline = -10f;
    [SerializeField] private float heightChangeSpeed = 12f;
    [SerializeField] private float gravity = -10f;

    [Header("Look")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    //[SerializeField] private string speedParam = "Speed";
    [SerializeField] private string movingParam = "IsWalking";
    [SerializeField] private string flyingParam = "IsFlying";
    [SerializeField] private float animSmooth = 10f;

    private float m_animSpeed;

    private float pitch;
    private float yaw;

    private CharacterController m_controller;
    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_flyAction;

    private Vector2 m_moveValue;
    private Vector2 m_lookValue;
    private float m_flyValue;

    private void OnEnable()
    {
        inputActions.FindActionMap(actionMapName).Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap(actionMapName).Disable();
    }

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();

        InputActionMap map = inputActions.FindActionMap(actionMapName, true);
        m_moveAction = map.FindAction(moveActionName, true);
        m_lookAction = map.FindAction(lookActionName, true);
        m_flyAction = map.FindAction(flyActionName, true);

        m_controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>();
        m_lookValue = m_lookAction.ReadValue<Vector2>();
        m_flyValue = m_flyAction.ReadValue<float>();
        Debug.Log(m_flyValue);

        RotateCamera();
        Move();
        Fly();
        ApplyGravity();
        UpdateFlight();
        UpdateAnimations();

    }

    private void Move()
    {
        Transform camera = cameraTransform;

        Vector3 forward = (camera != null) ? camera.forward : transform.forward;
        Vector3 right = (camera != null) ? camera.right : transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 dir = right * m_moveValue.x + forward * m_moveValue.y;

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * 360f * Time.deltaTime);
        }
        
        m_controller.Move(dir * walkSpeed * Time.deltaTime);
    }

    private void Fly() 
    {
        Vector3 up = transform.up;

        if (m_flyValue == 0.0f) return;

        Debug.Log(m_flyValue);
        Debug.Log(m_controller.isGrounded);
        Debug.Log(isFlying);

        if (!isFlying && m_flyValue >= 0.0f)
        {
            isFlying = true;
            UpdateAnimations();
        }

        m_controller.Move(up * m_flyValue * heightChangeSpeed * Time.deltaTime);

    }

    private void ApplyGravity()
    {
        Vector3 up = transform.up;
        if (isFlying || m_controller.isGrounded) return;

        m_controller.Move(up * gravity * Time.deltaTime);
    }

    private void UpdateFlight()
    {
        if (m_controller.isGrounded)
            isFlying = false;
        
    }

    private void RotateCamera()
    {
        if (cameraTarget == null) return;

        yaw += m_lookValue.x * rotateSpeed * Time.deltaTime;

        pitch -= m_lookValue.y * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        if (!isFlying)
        {
            float target = Mathf.Clamp01(m_moveValue.magnitude);
            m_animSpeed = Mathf.Lerp(m_animSpeed, target, animSmooth * Time.deltaTime);

            //animator.SetFloat(speedParam, m_animSpeed);
            animator.SetBool(flyingParam, false);
            animator.SetBool(movingParam, m_animSpeed > 0.05f);
        }

        if (isFlying)
        {
            animator.SetBool(flyingParam, true);
        }
    }
}