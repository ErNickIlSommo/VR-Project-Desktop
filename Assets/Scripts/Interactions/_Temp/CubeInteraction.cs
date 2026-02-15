using UnityEngine;

public class CubeInteraction : MonoBehaviour, IInteractable
{
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interact");
        return true;
    }
}
