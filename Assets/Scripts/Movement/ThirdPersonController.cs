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

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;

    [Header("Look")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    private float pitch;
    private float yaw;

    private CharacterController m_controller;
    private InputAction m_moveAction;
    private InputAction m_lookAction;

    private Vector2 m_moveValue;
    private Vector2 m_lookValue;

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

        var map = inputActions.FindActionMap(actionMapName, true);
        m_moveAction = map.FindAction(moveActionName, true);
        m_lookAction = map.FindAction(lookActionName, true);


        m_controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        m_moveValue = m_moveAction.ReadValue<Vector2>();
        m_lookValue = m_lookAction.ReadValue<Vector2>();

        Move();
        RotateCamera();

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
            transform.forward = dir.normalized;

        m_controller.Move(dir * walkSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (cameraTarget == null) return;

        yaw += m_lookValue.x * rotateSpeed * Time.deltaTime;

        pitch -= m_lookValue.y * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}