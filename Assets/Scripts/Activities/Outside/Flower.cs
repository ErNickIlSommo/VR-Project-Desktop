using System;
using UnityEngine;

public class Flower : MonoBehaviour, IInteractable
{
    public Action<bool, Flower> OnInteraction;
    
    [SerializeField] AudioSource m_AudioSource;

    private FlowerState flowerState;
    
    private bool _canInteract = false;


    private void Awake()
    {
        
    }

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
        Debug.Log("FLOWER " + gameObject.name + " Interaction");
        if (!_canInteract) return false;
        if (OnInteraction != null) OnInteraction.Invoke(true, this);
        return true;
    }

    public void BlockMovement(Interactor interactor)
    {
        
    }

    public void UnlockMovement(Interactor interactor)
    {
        
    }
}
