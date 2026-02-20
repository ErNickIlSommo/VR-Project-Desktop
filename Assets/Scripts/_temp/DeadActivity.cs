using System;
using UnityEngine;
using System.Collections.Generic;

public class DeadActivity: MonoBehaviour, Activity
{
    public Action<bool> ActivityFinished;
    
    [SerializeField] private ActivityTrigger _trigger;
    [SerializeField] private Abyss abyss;
    [SerializeField] private List<GameObject> corpses;
    
    [SerializeField] private bool _isActivityEnabled = false;
    [SerializeField] private bool _isActivityStarted = false;
    [SerializeField] private bool _isActivityCompleted = false;


    [SerializeField] private int _corpsesFound = 0;

    private void Awake()
    {
        _trigger.Activity = this;
        _trigger.DisableInteraction();
        
        foreach (GameObject corps in corpses) corps.SetActive(false);

        abyss.OnCorpseEntered += DestroyCorpse;
    }

    public void EnableActivity()
    {
        _isActivityEnabled = true;
        _trigger.EnableInteraction();
    }
    
    public bool StartActivity()
    {
        if (!_isActivityEnabled) return false;
        if (_isActivityStarted) return false;
        if (_isActivityCompleted) return false;

        _corpsesFound = 0;
        
        _trigger.DisableInteraction();
        foreach (GameObject corps in corpses) corps.SetActive(true);
        Debug.Log("DEAD ACTIVITY Started"); 
        return true;
    }

    private void DestroyCorpse(bool status)
    {
        if (!status) return;
        Debug.Log("DEAD ACTIVITY: Corpse entered");
        
        _corpsesFound++;
        if (_corpsesFound < corpses.Count) return;
        _isActivityStarted = false;
        _isActivityCompleted = true;
    }
    
}