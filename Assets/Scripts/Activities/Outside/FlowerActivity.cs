
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlowerActivity: MonoBehaviour, Activity
{
    public Action<bool> ActivityRunning;
    public Action<bool> ActivityFinished;
    
    // [SerializeField] private ActivityTrigger _trigger;
    [SerializeField] private List<Flower> flowers;

    [SerializeField] private bool _isActivityEnabled = false;
    [SerializeField] private bool _isActivityStarted = false;
    [SerializeField] private bool _isActivityCompleted = false;

    [SerializeField] private int _pollinatedFlowers = 0;

    private void Awake()
    {
        // _trigger.Activity = this;
        // _trigger.DisableInteraction();

        foreach (Flower flower in transform.GetComponentsInChildren<Flower>())
        {
            flowers.Add(flower);
            flower.OnInteraction += FlowerPollened;
            flower.DisableInteraction();
        }
    }

    public bool StartActivity()
    {
        if (!_isActivityEnabled) return false;
        if (_isActivityStarted) return false;
        if (_isActivityCompleted) return false;

        _pollinatedFlowers = 0;
        
        // _trigger.DisableInteraction();
        _isActivityStarted = true;
        
        foreach (Flower flower in flowers) flower.EnableInteraction();

        return true;
    }

    public void EnableActivity()
    {
        _isActivityEnabled = true;
        // _trigger.EnableInteraction();
    }

    private void FlowerPollened(bool status, Flower flower)
    {
        if (!_isActivityEnabled) return;
        if (_isActivityCompleted) return;
        if (!_isActivityStarted) return;
        if (!status) return;

        _pollinatedFlowers++;
        flower.DisableInteraction();
        if (_pollinatedFlowers < flowers.Count)
        {
            if (ActivityRunning != null) ActivityRunning.Invoke(true);
            return;
        }
        _isActivityStarted = false;
        _isActivityCompleted = true;
        _isActivityEnabled = false;
        if (ActivityFinished != null) ActivityFinished.Invoke(_isActivityCompleted);
    }
}