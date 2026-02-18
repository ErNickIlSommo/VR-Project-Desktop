using UnityEngine;

public class CubeInteraction : MonoBehaviour, IInteractable
{
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interact");
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
