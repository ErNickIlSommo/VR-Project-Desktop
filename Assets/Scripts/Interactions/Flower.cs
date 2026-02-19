using System;
using UnityEngine;

public class Flower : MonoBehaviour, IInteractable
{
    public Action<bool> OnInteractionFinished;
    
    private bool _canInteract = true;
    
    private GameObject _flowerObject;

    public GameObject FlowerObject => gameObject;
    
    public bool Interact(Interactor interactor)
    {
        if (!_canInteract)
        {
            Debug.Log("FLOWER: Can't interact with this flower");
            return false;
        }
        
        Debug.Log("FLOWER: Interaction good");
        _canInteract = false;
        if (OnInteractionFinished != null)
            OnInteractionFinished.Invoke(true);
        
        return true;
    }

    public void BlockMovement(Interactor interactor)
    {
        // throw new System.NotImplementedException();
    }

    public void UnlockMovement(Interactor interactor)
    {
        // throw new System.NotImplementedException();
    }
}
