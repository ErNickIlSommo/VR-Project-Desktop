using UnityEngine;

public interface IInteractable
{
    public bool Interact(Interactor interactor);
    public void BlockMovement(Interactor interactor);
    public void UnlockMovement(Interactor interactor);

}
