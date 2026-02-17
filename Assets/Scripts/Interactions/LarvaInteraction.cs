using System;
using UnityEngine;
using System.Collections;

public class LarvaInteraction : MonoBehaviour, IInteractable
{
    // Event for signaling to master
    public event Action<LarvaEventInfo> OnRequestTerminated;
    
    // Identifier (assigned by master)
    private int _larvaId;
    
    // Request and received Objects
    private GrabbableObjectData _requestedObject;
    private GrabbableObjectData _receivedObject;
    
    // Time variables
    [SerializeField] private float _cooldown = 10f;
    [SerializeField] private float _timer = 0f;
    private Coroutine _timerRunningCoroutine; 
    
    // Flag controlling the request
    [SerializeField] private bool _isRequestRunning = false; 
    [SerializeField] private bool _isRequestCorrect;

    /*
     * Test functionalities
     */
    [SerializeField] private Renderer renderer;
    private Color _defaultColor = Color.darkGray;
    private Color _requestColor = Color.blue;
    private Color _correctColor = Color.green;
    private Color _incorrectColor = Color.red;
    

    // Getters and Setters
    
    
    public int LarvaId { get => _larvaId; set => _larvaId = value; }
    
    public GrabbableObjectData RequestedObject
    {
        get => _requestedObject;
        set => _requestedObject = value;
    } 
    
    public float Cooldown
    {
        get => _cooldown;
        set => _cooldown = value;
    }  
    
    public bool IsRequestRunning => _isRequestRunning;
    public bool IsRequestCorrect => _isRequestCorrect;
    
    // Methods

    /*
     * Test Functionalities
     */
    public void InitLarva()
    {
        renderer.material.color = _defaultColor;
    }

    public bool StartRequest(GrabbableObjectData requestedObject, float cooldown = float.NaN)
    {
        if (_timerRunningCoroutine != null) return false; 
        if (requestedObject == null) return false; 
        if (!float.IsNaN(cooldown)) _cooldown = cooldown;
        
        if (!float.IsNaN(cooldown)) _cooldown = cooldown;
        
        _isRequestCorrect = false; 
        _timerRunningCoroutine = StartCoroutine(Timer());
        _requestedObject = requestedObject;
        _isRequestRunning = true; 
        
        /*
         * Test functionalities
         */
        Debug.Log("Started request item: " + _requestedObject.Name);
        renderer.material.color = _requestColor;
        
        return true;
    }
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Larva Interaction started");
        
        if (!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        
        Debug.Log("Player is grabbing something"); 
        
        if (!IsCorrectIngredient(interactor))
        {
            _isRequestCorrect = false;
            RefuseIngredient();
            interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);
            return false;
        }

        _isRequestCorrect = true;
        interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);

        return true;
    }

    private bool IsCorrectIngredient(Interactor interactor)
    {
        if(!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        if (!interactor.PlayerInteractionStatus.ObjectData) return false;
        
        int objectDataId = interactor.PlayerInteractionStatus.ObjectData.Id;
        
        return _requestedObject.Id == objectDataId;
    }

    private void RefuseIngredient()
    {
        if (_timerRunningCoroutine != null)
        {
            StopCoroutine(_timerRunningCoroutine);
            _timerRunningCoroutine = null;
        }
        
        _requestedObject = null;
        _isRequestRunning = false;

        /*
         * Test functionalities
         */
        Debug.Log("Task failed");
        renderer.material.color = _incorrectColor;
        
        if (OnRequestTerminated != null) 
            OnRequestTerminated.Invoke(
                new LarvaEventInfo(
                    _larvaId,
                    transform.gameObject.name,
                    false
            ));
    }

    private void EatIngredient()
    {
        _requestedObject = null;
        _timerRunningCoroutine = null;
        _isRequestRunning = false;
        
        /*
         * Test functionalities
         */
        Debug.Log("Task succeded");
        renderer.material.color = _correctColor;
        
        if (OnRequestTerminated != null) 
            OnRequestTerminated.Invoke(
                new LarvaEventInfo(
                    _larvaId,
                    transform.gameObject.name,
                    true
            ));
    }
    
    private IEnumerator Timer()
    {
        _timer = 0f;

        while (_timer < _cooldown)
        {
            if (_isRequestCorrect)
            {
                EatIngredient();
                _timerRunningCoroutine = null;
                yield break;
            }
            
            _timer += Time.deltaTime;
            yield return null;
        }

        RefuseIngredient();
        _timerRunningCoroutine = null;
    }
}
