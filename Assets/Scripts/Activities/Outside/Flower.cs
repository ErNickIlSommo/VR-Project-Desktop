using System;
using UnityEngine;

public class Flower : MonoBehaviour, IInteractable
{
    [SerializeField] AudioSource m_AudioSource;

    private FlowerState flowerState;


    private void Awake()
    {
        
    }

    public bool Interact(Interactor interactor)
    {
        return false;
    }

    public void BlockMovement(Interactor interactor)
    {
        
    }

    public void UnlockMovement(Interactor interactor)
    {
        
    }
}
