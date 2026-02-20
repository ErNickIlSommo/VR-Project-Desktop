using UnityEngine;

public class Spawner: MonoBehaviour, IInteractable
{
    private bool _canInteract;
    
    public void EnableInteraction()
    {
        _canInteract = true;
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }
    
    public bool Interact(Interactor interactor)
    {
        if (!_canInteract) return false;
        return true;
    }

    public void BlockMovement(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public void UnlockMovement(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }
}