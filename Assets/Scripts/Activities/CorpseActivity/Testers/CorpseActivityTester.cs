using System;
using UnityEngine;

public class CorpseActivityTester : MonoBehaviour
{
    [SerializeField] private bool startActivity;
    
    private CorpseActivity _corpseActivity;

    private void Awake()
    {
        _corpseActivity = GetComponent<CorpseActivity>();
        _corpseActivity.OnActivityStarted += SignalStartedActivity;
        _corpseActivity.OnActivityCompleted += SignalCompletedActivity;
    }

    private void Update()
    {
        if (_corpseActivity.IsActivityRunning) return;
        if (!startActivity) return;
        
        startActivity = false;
        _corpseActivity.StartActivity();
    }

    private void SignalStartedActivity(bool started)
    {
        Debug.Log("Corpse Activity Started");
    }

    private void SignalCompletedActivity(bool completed)
    {
        Debug.Log("Corpse Activity Completed");
    }
}

