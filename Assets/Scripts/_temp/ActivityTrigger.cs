using System;
using UnityEngine;

public class ActivityTrigger: MonoBehaviour, IInteractable
{
    private Activity _activity;
    [SerializeField] private bool _canInteract = false;

    private Interactor _interactor;
    
    public Activity Activity { get { return _activity; } set { _activity = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canInteract) return;
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered");
        // Update ui
        Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_canInteract) return;
        if (!other.CompareTag("Player")) return;
        // update UI
    }

    public bool Interact(Interactor interactor)
    {
        // Debug.Log("Trigger interaction");
        if(!_canInteract) return false;
        _activity.StartActivity();
        _interactor = interactor;
        interactor.InteractionTrigger.RemoveInteractable(this);
        return true;
    }
    
    public void EnableInteraction()
    {
        Debug.Log("Trigger enabled");
        _canInteract = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void DisableInteraction()
    {
        Debug.Log("Trigger disabled");
        _canInteract = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void BeDetectable()
    {
        _interactor.InteractionTrigger.AddInteractable(this);
    }
    
    public void BlockMovement(Interactor interactor)
    {
    }
    public void UnlockMovement(Interactor interactor)
    {
    }
}