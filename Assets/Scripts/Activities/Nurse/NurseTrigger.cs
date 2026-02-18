using System;
using TMPro;
using UnityEngine;

public class NurseTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private NurseActivity nurseActivity;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private bool _canTrigger;
    private bool _isAlreadyTriggered;
    
    private void Awake()
    {
        _canTrigger = nurseActivity.CanStartActivity;
        _isAlreadyTriggered = false;
        panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        Debug.Log("Player Entered in Nurse Trigger");
        
        _canTrigger = nurseActivity.CanStartActivity;
        if (!_canTrigger) return;
        if (_isAlreadyTriggered) return;
        panel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
            
        Debug.Log("Player Exited in Nurse Trigger");
        
        _canTrigger = nurseActivity.CanStartActivity;
        if (!_canTrigger) return;
        if(_isAlreadyTriggered) return;
        
        panel.SetActive(false);
    }

    public bool Interact(Interactor interactor)
    {
        _canTrigger = nurseActivity.CanStartActivity;
        if (!_canTrigger) return false;
        
        nurseActivity.StartActivity();
        _canTrigger = false;
        _isAlreadyTriggered = true;
        panel.SetActive(false);  
        
        // gameObject.SetActive(false);
        gameObject.layer = 0;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        interactor.InteractionTrigger.RemoveInteractable(this);

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
