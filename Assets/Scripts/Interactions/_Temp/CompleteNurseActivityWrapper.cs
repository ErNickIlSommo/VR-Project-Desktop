using UnityEngine;

public class CompleteNurseActivityWrapper : MonoBehaviour
{
    [SerializeField] private NurseBeeDialogue _dialogue;

    [SerializeField] private bool _activated = false;

    private bool _activatedGuard = false;

    private void Awake()
    {
        _dialogue = GetComponent<NurseBeeDialogue>();
    }

    private void Update()
    {
        if (!_activated) return;
        if (_activatedGuard) return;
        
        _dialogue.HasCompletedActivity = true;
        _activatedGuard = true;
    }
}
