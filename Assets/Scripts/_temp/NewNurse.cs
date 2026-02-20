using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewNurse : MonoBehaviour, Activity
{
    public Action<bool> ActivityFinished;
    
    [SerializeField] private List<NewLarva> larvas;
    [SerializeField] private List<FoodSpawner> spawners;
    [SerializeField] private ActivityTrigger _trigger;
    

    [SerializeField] private bool _isActivityEnabled = false;
    [SerializeField] private bool _isActivityStarted = false;
    [SerializeField] private bool _isActivityCompleted = false;
    
    [SerializeField] private float cooldown = 13f;
    [SerializeField] private int maxErrors = 3;
    [SerializeField] private int correct = 0;
    [SerializeField] private int errors = 0;
    private int _numShuffles = 5000;

    [SerializeField] private List<GrabbableObjectData> food;

    private void Awake()
    {
        _trigger.Activity = this;
        _trigger.DisableInteraction();
        
        foreach (var larva in transform.GetComponentsInChildren<NewLarva>())
        {
            larvas.Add(larva);
            larva.DisableInteraction();
        }

        foreach (var spawner in transform.GetComponentsInChildren<FoodSpawner>())
        {
            spawners.Add(spawner);
            spawner.DisableInteraction();
        }
        
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

        correct = 0;
        errors = 0;
        
        _trigger.DisableInteraction();
        foreach (FoodSpawner spawner in spawners) spawner.EnableInteraction();
        
        ShuffleLarvas();
        _isActivityStarted = true;
        Debug.Log("Activity Started");
        StartCoroutine(Requests());
        
        return true;
    }

    private void ShuffleLarvas()
    {
        for (int i = 0; i < _numShuffles; i++)
        {
            int rd1 = Random.Range(0, larvas.Count);
            int rd2 = Random.Range(0, larvas.Count);
            (larvas[rd1], larvas[rd2]) = (larvas[rd2],  larvas[rd1]);
        }
    }

    private IEnumerator Requests()
    {
        bool outcome = true;
        foreach (NewLarva larva in larvas)
        {
            var rd = Random.Range(0, food.Count);
            
            larva.EnableInteraction();
            yield return larva.SendFoodRequest(cooldown, food[rd]);
            larva.DisableInteraction();
            
            outcome = CheckRequestOutcome(larva);
            if (!outcome)
            {
                EvaluateActivity(outcome);
                yield break;
            }
        }

        EvaluateActivity(outcome);

    }

    private bool CheckRequestOutcome(NewLarva larva)
    {
        switch (larva.Status)
        {
            case NewLarva.RequestStatus.STARTED:
                // should never be inside this
                break;
            case NewLarva.RequestStatus.CORRECT:
                correct++;
                break;
            case NewLarva.RequestStatus.WRONG:
                errors++;
                break;
        }

        if (errors < maxErrors) return true;
        return false;
    }

    private void EvaluateActivity(bool outcome)
    {
        if (outcome)
        {
            _isActivityCompleted = true;
            _isActivityStarted = false;
            foreach (var spawner in spawners) spawner.DisableInteraction();
            if (ActivityFinished != null) ActivityFinished.Invoke(_isActivityCompleted);
            return;
        }

        _trigger.EnableInteraction();
        _trigger.BeDetectable();
        foreach (var spawner in spawners) spawner.DisableInteraction();
        
        _isActivityStarted = false;
    }
    
}
