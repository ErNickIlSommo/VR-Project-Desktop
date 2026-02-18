using System;
using TMPro;
using UnityEngine;

public class NurseTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private NurseActivity nurseActivity;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject foodSpawner1;
    [SerializeField] private GameObject foodSpawner2;
    [SerializeField] private GameObject foodSpawner3;

    private bool _canTrigger;
    private bool _isAlreadyTriggered;
    
    private Interactor _interactor;
    
    private void Awake()
    {
        _canTrigger = nurseActivity.CanStartActivity;
        _isAlreadyTriggered = false;
        panel.SetActive(false);

        foodSpawner1.layer = 0;
        foodSpawner2.layer = 0;
        foodSpawner3.layer = 0;

        nurseActivity.OnActivityCompleted += HandleCompletedActivity;
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
        foodSpawner1.layer = 6;
        foodSpawner2.layer = 6;
        foodSpawner3.layer = 6;
        
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        _interactor = interactor;
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

    private void HandleCompletedActivity(bool status)
    {
        Debug.Log("Trigger Received Completed Activity: " + status);
        if (status) return;
        
        _canTrigger = nurseActivity.CanStartActivity;
        _isAlreadyTriggered = false; 
        panel.SetActive(false);
        
        foodSpawner1.layer = 0;
        foodSpawner2.layer = 0;
        foodSpawner3.layer = 0;
        
        gameObject.layer = 6;
        // _interactor.InteractionTrigger.AddInteractable(this);
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;
    }
}
