using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private InteractorTrigger _interactionTrigger;

    [Header("Actions")]
    [SerializeField] private InputActionAsset inputActions;

    private PlayerInteractionStatus _playerInteractionStatus;
    public InputActionAsset InputActions => inputActions;

    public PlayerInteractionStatus PlayerInteractionStatus => _playerInteractionStatus;
    
    public InteractorTrigger InteractionTrigger => _interactionTrigger;

    private void Awake()
    {
        _playerInteractionStatus = GetComponent<PlayerInteractionStatus>();
    }

    private void OnInteract()
    {
        IInteractable interactable = _interactionTrigger.GetInteractable();

        if (interactable == null)
        {
            if (CheckIfHasGrabbedObject()) _playerInteractionStatus.GrabbableObject.ForceDrop(this);
            return;
        }
        
        interactable.Interact(this);

        // se oggetto grabbabile devo triggerare animazione del player (distinguere fra grab e drop)
    }

    private bool CheckIfHasGrabbedObject()
    {
        return _playerInteractionStatus.HasGrabbed;
    }
}
