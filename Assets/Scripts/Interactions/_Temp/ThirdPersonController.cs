using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interact;    // Button

    [Header("Refs")]
    public Transform cameraTransform;        // Camera main (o pivot/camera root)

    private CharacterController cc;
    
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main) cameraTransform = Camera.main.transform;
    }

    void OnEnable()
    {
        if (interact)
        {
            interact.action.Enable();
            interact.action.performed += OnInteract;
        }
    }

    void OnDisable()
    {
        if (interact) interact.action.performed -= OnInteract;

        if (interact) interact.action.Disable();
    }
    void OnInteract(InputAction.CallbackContext _)
    {
        // Qui ci agganci la tua logica: raycast/overlap + chiamata Interact()
        //Debug.Log("Interact pressed");
    }
    
}
