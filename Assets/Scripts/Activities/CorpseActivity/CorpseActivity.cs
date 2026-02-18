using System;
using System.Collections.Generic;
using UnityEngine;

public class CorpseActivity : MonoBehaviour
{
    public event Action<bool> OnActivityStarted;
    public event Action<bool> OnActivityCompleted;
    
    [SerializeField] Abyss abyss;
    [SerializeField] private bool isActivityRunning = false;
    [SerializeField] private bool isActivityCompleted = false;
    [SerializeField] private int corpseCount = 0;
    [SerializeField] private int totalCorpseCount;

    [SerializeField] private List<GrabbableObject> corpses;
    
    public bool IsActivityRunning => isActivityRunning;
    
    private void Awake()
    {
        isActivityRunning = false;
        isActivityCompleted = false;
        totalCorpseCount = corpses.Count;
        abyss.OnCorpseEntered += HandleCorpseEntered;
    }

    public void StartActivity()
    {
        corpseCount = 0;
        totalCorpseCount = corpses.Count;
        isActivityRunning = true;
        isActivityCompleted = false;
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
}
