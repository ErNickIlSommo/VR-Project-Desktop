using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

public class Flower : MonoBehaviour, IInteractable
{
    public Action<bool, Flower> OnInteraction;

    [SerializeField] private float timeUIToDisappear = 2f;
    [SerializeField] private AudioClip m_AudioClip;
    private AudioSource m_AudioSource;
    private FlowerUI _ui;

    private FlowerState flowerState;
    
    private bool _canInteract = false;


    private void Awake()
    {
        _ui = GetComponent<FlowerUI>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void EnableInteraction()
    {
        _canInteract = true;
        _ui.UpdateImage(FlowerState.ToForage);
        _ui.Show();
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("FLOWER " + gameObject.name + " Interaction");
        if (!_canInteract) return false;
        StartCoroutine(WaitAndUpdateUI());
        m_AudioSource.PlayOneShot(m_AudioClip);
        interactor.PlayerInteractionStatus.DoPollenInteraction();

        if (OnInteraction != null) OnInteraction.Invoke(true, this);
        return true;
    }

    private IEnumerator WaitAndUpdateUI()
    {
        _ui.UpdateImage(FlowerState.Foraged);
        yield return new WaitForSeconds(timeUIToDisappear);
        _ui.Hide();
    }

    public void BlockMovement(Interactor interactor)
    {
        
    }

    public void UnlockMovement(Interactor interactor)
    {
        
    }
}
