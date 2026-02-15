using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float  _interactionDistance;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InputActionAsset inputActions;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private PlayerInteractionStatus _playerInteractionStatus;
    public InputActionAsset InputActions => inputActions;

    public PlayerInteractionStatus PlayerInteractionStatus => _playerInteractionStatus;

    private void Awake()
    {
        _playerInteractionStatus = GetComponent<PlayerInteractionStatus>();
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionDistance, _colliders, _interactableMask);
    }

    private void OnInteract()
    {
        if (_numFound == 0) return;

        var interactable = _colliders[0].GetComponent<IInteractable>();

        if (interactable == null) return;
        if (!Keyboard.current.eKey.wasPressedThisFrame) return;

        interactable.Interact(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionDistance);
    }
}
