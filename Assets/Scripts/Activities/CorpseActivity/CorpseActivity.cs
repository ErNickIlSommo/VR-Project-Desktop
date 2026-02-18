using System;
using System.Collections.Generic;
using UnityEngine;

public class CorpseActivity : MonoBehaviour
{
    public event Action<bool> OnActivityStarted;
    public event Action<bool> OnActivityCompleted;
    
    [SerializeField] Abyss abyss;
    
    private bool _canStartActivity;
    [SerializeField] private bool isActivityRunning = false;
    [SerializeField] private bool isActivityCompleted = false;
    [SerializeField] private int corpseCount = 0;
    [SerializeField] private int totalCorpseCount;

    [SerializeField] private List<GameObject> corpses;
    
    
    public bool CanStartActivity { get => _canStartActivity; set => _canStartActivity = value; }
    
    public bool IsActivityRunning => isActivityRunning;
    
    private void Awake()
    {
        _canStartActivity = false;
        isActivityRunning = false;
        isActivityCompleted = false;
        totalCorpseCount = corpses.Count;
        
        // Disable Objects
        SetUpCorpse(false);
        
        abyss.OnCorpseEntered += HandleCorpseEntered;
    }

    public void StartActivity()
    {
        if (!_canStartActivity)
        {
            Debug.LogError("Can't start activity");
            return;
        }
        corpseCount = 0;
        totalCorpseCount = corpses.Count;
        isActivityRunning = true;
        isActivityCompleted = false;
        
        SetUpCorpse(true);
        
        if (OnActivityStarted != null)
            OnActivityStarted.Invoke(true);
    }

    private void StopActivity()
    {
        isActivityRunning = false;
    }

    private void HandleCorpseEntered(bool isEntered)
    {
        Debug.Log("Received feedback");
        corpseCount++;
        if (OnActivityCompleted != null && corpseCount == totalCorpseCount)
        {
            isActivityCompleted = true;
            OnActivityCompleted.Invoke(true);
            StopActivity();
        }
    }

    private void SetUpCorpse(bool status)
    {
        for (int i = 0; i < corpses.Count; i++)
        {
            corpses[i].SetActive(status);
        } 
    }
}