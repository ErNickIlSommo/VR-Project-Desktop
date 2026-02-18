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

    [SerializeField] private Transform visualPivot;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Flight")]
    [SerializeField] private float heightChangeSpeed = 11f;
    [SerializeField] private float gravity = -10f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string movingParam = "IsWalking";
    [SerializeField] private string flyingParam = "IsFlying";
    [SerializeField] private string transitionUpParam = "IsTransitionUp";
    [SerializeField] private string transitionDownParam = "IsTransitionDown";
    [SerializeField] private float animSmooth = 10f;

    private CharacterController m_controller;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_flyAction;

    private Vector2 m_moveValue;
    private float m_flyValue;
    private float m_animSpeed;

    private enum MovementState { Idle, Walk, Fly, FreeFall, GroundUpToWall, Climb }
    [SerializeField] private MovementState movementState = MovementState.Idle;

    private void OnEnable() => inputActions.FindActionMap(actionMapName).Enable();
    private void OnDisable() => inputActions.FindActionMap(actionMapName).Disable();

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();

        var map = inputActions.FindActionMap(actionMapName, true);
        m_moveAction = map.FindAction(moveActionName, true);
        m_lookAction = map.FindAction(lookActionName, true);
        m_flyAction = map.FindAction(flyActionName, true);

        if (visualPivot == null)
            visualPivot = transform;
    }

    private void Update()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>();
        m_flyValue = m_flyAction.ReadValue<float>();

        Move();
        Fly();
        ApplyGravity();
        UpdateFlight();

        UpdateAnimations();
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
        if (movementState == MovementState.Fly || movementState == MovementState.Climb) return;

        Vector3 dir = GetMoveDirWorld();

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * 360f * Time.deltaTime);
        }

        m_controller.Move(dir * walkSpeed * Time.deltaTime);

        if (m_controller.isGrounded && movementState == MovementState.Idle)
            movementState = MovementState.Walk;
    }

    private void Fly()
    {
        if (m_flyValue == 0.0f) return;

        if (movementState != MovementState.Fly && m_flyValue >= 0.0f)
            movementState = MovementState.Fly;

        m_controller.Move(Vector3.up * m_flyValue * heightChangeSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!m_controller.isGrounded && movementState != MovementState.Fly && movementState != MovementState.Climb)
            m_controller.Move(Vector3.up * gravity * Time.deltaTime);
    }

    private void UpdateFlight()
    {
        if (!m_controller.isGrounded && (movementState == MovementState.Walk || movementState == MovementState.Idle))
            movementState = MovementState.FreeFall;

        if (m_controller.isGrounded && movementState == MovementState.FreeFall)
            movementState = MovementState.Idle;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        bool inTransition = movementState == MovementState.GroundUpToWall;

        if (inTransition)
        {
            animator.SetBool(flyingParam, false);
            animator.SetBool(movingParam, false);
            return;
        }

        if (movementState == MovementState.Climb)
        {
            animator.SetBool(flyingParam, false);
            animator.SetBool(movingParam, true);
            return;
        }

        if (movementState == MovementState.Fly)
        {
            animator.SetBool(flyingParam, true);
            animator.SetBool(movingParam, false);
            return;
        }

        float target = Mathf.Clamp01(m_moveValue.magnitude);
        m_animSpeed = Mathf.Lerp(m_animSpeed, target, animSmooth * Time.deltaTime);

        animator.SetBool(flyingParam, false);
        animator.SetBool(movingParam, m_animSpeed > 0.05f);
    }
}