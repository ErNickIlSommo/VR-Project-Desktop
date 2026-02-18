using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NurseActivity : MonoBehaviour
{
    public event Action<bool> OnActivityStarted;
    public event Action<bool> OnActivityCompleted;
    
    [SerializeField] private bool _isActivityStarted = false;
    [SerializeField] private bool _isActivityCompleted = false;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _failedRequests = 0;
    
    [SerializeField] private float[] _cooldowns = { 20f, 15f, 13f, 9f };
    [SerializeField] private int _lvl1 = 6;
    [SerializeField] private int _lvl2 = 9;
    [SerializeField] private int _lvl3 = 11;
    
    [SerializeField] private float _currentCooldown;

    [SerializeField] private Larvas larvasManager;
    [SerializeField] private List<GrabbableObjectData> larvaRequestOptions;

    private bool _isForced;
    
    // Getters and Setters
    
    
    public bool IsActivityStarted 
    {
        get { return _isActivityStarted; }
        set { _isActivityStarted = value; }
    }
    
    public bool IsActivityCompleted => _isActivityCompleted;
    
    public int Score => _score;

    
    // Methods

    private void Awake()
    {
        larvasManager.SendInfoToMaster += HandleLarvaTerminated;
        larvasManager.IsLarvasTerminated += HandleAllLarvasTerminated;
    }

    public void StartActivity(int howMany = 5000)
    {
        if (_isActivityStarted) return;
        
        if (howMany <= 0) howMany = 5000;
        
        
        _score = 0;
        _failedRequests = 0;
        _currentCooldown = _cooldowns[0];
        larvasManager.InitLarvasManager();
        _isActivityCompleted = false;
        _isActivityStarted = true;
        _isForced = false;
        
        if(OnActivityStarted != null) OnActivityStarted.Invoke(_isActivityCompleted);
        
        // Generate a request with a random Ingredient
        int ingredientIndex = Random.Range(0, larvaRequestOptions.Count);
        larvasManager
            .SendRequestToLarvas(
                larvaRequestOptions[ingredientIndex],
                _currentCooldown
            );
    }

    private void HandleLarvaTerminated(LarvaEventInfo eventInfo)
    {
        if (!_isActivityStarted) return;
        if (eventInfo == null) return;
        
        // If the Player is dumb
        if (!eventInfo.RequestResutlt)
        {
            _failedRequests++;
            if (_failedRequests >= 3)
            {
                StopActivity();
                return;
            }
        } else _score++;
        
        // Generate another request
        var level = larvasManager.Index + 1;
        if(level >= _lvl1) _currentCooldown = _cooldowns[1];
        if (level >= _lvl2) _currentCooldown = _cooldowns[2];
        if (level >= _lvl3) _currentCooldown = _cooldowns[3];
        // else _currentCooldown = _cooldowns[0];
        
        int ingredientIndex = Random.Range(0, larvaRequestOptions.Count);
        larvasManager
            .SendRequestToLarvas(
                larvaRequestOptions[ingredientIndex],
                _currentCooldown
            );
    }

    private void HandleAllLarvasTerminated(bool status)
    {
        if (!status) return;
        StopActivity();
    }
    
    public void StopActivity()
    {
        if (!_isActivityStarted) return;
        
        if (_score >= larvasManager.NurseLarvas.Count - 3)
            _isActivityCompleted = true;
        else _isActivityCompleted = false;
        
        _isActivityStarted = false;
        
        if (OnActivityCompleted != null) OnActivityCompleted.Invoke(_isActivityCompleted);
    }
}
