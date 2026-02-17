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

    private void Awake()
    {
        _playerInteractionStatus = GetComponent<PlayerInteractionStatus>();
    }

    private void OnInteract()
    {
        if (CheckIfHasGrabbedObject())
        {
            _playerInteractionStatus.GrabbableObject.ForceDrop(this);
            return;
        } 
        
        IInteractable interactable = _interactionTrigger.GetInteractable();

        if (interactable == null) return;

        interactable.Interact(this);

        // se oggetto grabbabile devo triggerare animazione del player (distinguere fra grab e drop)
    }

    private bool CheckIfHasGrabbedObject()
    {
        return _playerInteractionStatus.HasGrabbed;
    }
}
