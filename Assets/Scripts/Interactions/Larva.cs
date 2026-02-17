using System;
using UnityEngine;
using System.Collections;

public class Larva : MonoBehaviour
{
    private GrabbableObjectData _requestedObject;
    public GrabbableObjectData RequestedObject
    {
        get => _requestedObject;
        set => _requestedObject = value;
    }

    private GrabbableObjectData _receivedObject;
    
    [SerializeField] private float _cooldown = 10f;
    public float Cooldown
    {
        get => _cooldown;
        set => _cooldown = value;
    }

    [SerializeField] private bool _isRequestRunning = false;
    public bool IsRequestRunning { get => _isRequestRunning; }

    private Coroutine _timerRunningCoroutine;
    [SerializeField] private bool _isRequestCorrect;

    [SerializeField] private float _timer = 0f;
    
    public bool StartRequest(GrabbableObjectData requestedObject,  float cooldown = float.NaN)
    {
        if (_timerRunningCoroutine != null) return false;
        if (requestedObject == null) return false;
        
        if (!float.IsNaN(cooldown)) _cooldown = cooldown;
        
        _isRequestCorrect = false;
        _timerRunningCoroutine = StartCoroutine(Timer());
        _requestedObject = requestedObject;
        _isRequestRunning = true;
        
        Debug.Log("Started request item with id: " + _requestedObject.Id);
        
        return true;
    }

    private bool Interact(GameObject gameObject)
    {
        if (!_isRequestRunning) return false;
        if (_timerRunningCoroutine == null) return false;
        if (_receivedObject == null) return false;

        // Debug.Log("Interacting item with id: " + _receivedObject.Id + " my id: " + _receivedObject.Id);
        if (_receivedObject.Id == _requestedObject.Id)
        {
            _isRequestCorrect = true;
            Destroy(gameObject);
            return true;
        }
        
        Destroy(gameObject);
        TaskFailed();
        return false;
    }

    // if(CompareTag(other.gameObject.tag) == "")
    // check if it's dropped
    // Destroy
    // Check if good or not
    // Handle animation
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("OnCollisionEnter: " + other.gameObject.name);
        if (!other.gameObject.CompareTag("Grabbable")) return;
        
        GrabbableObject grabbableObject = other.gameObject.GetComponent<GrabbableObject>();
        if (!grabbableObject) return;

        
        // Debug.Log("Grabbable Object with ID: " + grabbableObject.ObjectData.Id);
        _receivedObject = grabbableObject.ObjectData;
        
        if (!grabbableObject.IsDropped) return;

        var res = Interact(other.gameObject);
    }

    private IEnumerator Timer()
    {
        _timer = 0f;

        while (_timer < _cooldown)
        {
            if (_isRequestCorrect)
            {
                TaskSucceded();
                _timerRunningCoroutine = null;
                yield break;
            }
            
            _timer += Time.deltaTime;
            yield return null;
        }

        TaskFailed();
        _timerRunningCoroutine = null;
    }

    private void TaskSucceded()
    {
        _requestedObject = null;
        _timerRunningCoroutine = null;
        _isRequestRunning = false;
        
        Debug.Log("Task succeded");
    }

    private void TaskFailed()
    {
        if (_timerRunningCoroutine != null)
        {
            StopCoroutine(_timerRunningCoroutine);
            _timerRunningCoroutine = null;
        }
        
        _requestedObject = null;
        _isRequestRunning = false;
        
        Debug.Log("Task failed");
    }
}

