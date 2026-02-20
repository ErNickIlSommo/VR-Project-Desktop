using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class LarvaInteraction : MonoBehaviour, IInteractable
{
    // Event for signaling to master
    public event Action<LarvaEventInfo> OnRequestTerminated;
    
    // Identifier (assigned by master)
    private int _larvaId;
    
    // Request and received Objects
    private GrabbableObjectData _requestedObject;
    private GrabbableObjectData _receivedObject;

    [SerializeField] private BillboardUI ui;
    
    // Time variables
    [SerializeField] private float _cooldown = 10f;
    [SerializeField] private float _timer = 0f;
    [SerializeField] private float _waitStartRequest = 3f;
    [SerializeField] private float _waitTimer = 0f;
    private Coroutine _timerRunningCoroutine; 
    private Coroutine _timerWaitingCoroutine;
    
    // Flag controlling the request
    [SerializeField] private bool _isRequestRunning = false; 
    [SerializeField] private bool _isRequestCorrect;

    /*
     * Test functionalities
     */
    // [SerializeField] private Renderer renderer;
    private Color _defaultColor = Color.darkGray;
    private Color _requestColor = Color.blue;
    private Color _correctColor = Color.green;
    private Color _incorrectColor = Color.red;

    [SerializeField] private LarvaAnimationController _animationController;

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
        // renderer.material.color = _defaultColor;
        _animationController.Reset();
        // ui.Restart();
    }

    public bool StartRequest(GrabbableObjectData requestedObject, float cooldown = float.NaN)
    {
        
        if (_timerRunningCoroutine != null) return false; 
        if (requestedObject == null) return false; 
        if (!float.IsNaN(cooldown)) _cooldown = cooldown;
        
        if (!float.IsNaN(cooldown)) _cooldown = cooldown;
        
        _requestedObject = requestedObject;
        _isRequestCorrect = false; 
        _isRequestRunning = true;

        _timerWaitingCoroutine = StartCoroutine(WaitForStartingRequest());
        
        
        
        return true;
    }
    
    public bool Interact(Interactor interactor)
    {
        if (!_isRequestRunning) return false;
        Debug.Log("Larva Interaction started");
        
        if (!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        
        Debug.Log("Player is grabbing something"); 
        
        if (!IsCorrectIngredient(interactor))
        {
            _isRequestCorrect = false;
            BlockMovement(interactor);
            RefuseIngredient();
            UnlockMovement(interactor);
            interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);
            return false;
        }

        BlockMovement(interactor);
        
        _isRequestCorrect = true;
        interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);
        
        UnlockMovement(interactor);

        return true;
    }

    private bool IsCorrectIngredient(Interactor interactor)
    {
        if(!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        if (!interactor.PlayerInteractionStatus.ObjectData) return false;
        
        int objectDataId = interactor.PlayerInteractionStatus.ObjectData.Id;
        
        Debug.Log("Id: " + objectDataId);
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
        _animationController.RefuseFood();

        /*
         * Test functionalities
         */
        Debug.Log("Task failed");
        ui.OnNope();
        // renderer.material.color = _incorrectColor;
        
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

        _animationController.AcceptFood();
        /*
         * Test functionalities
         */
        Debug.Log("Task succeded");
        ui.Ok();
        // renderer.material.color = _correctColor;
        
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

    private IEnumerator WaitForStartingRequest()
    {
        _animationController.RequestFood();
        // renderer.material.color = _requestColor;
        Debug.Log(gameObject.name + ":  !!!");
        ui.OnRequest(); 
        
        _waitTimer = 0f;
        while (_waitTimer < _waitStartRequest)
        {
            _waitTimer += Time.deltaTime;
            yield return null;
        }
        
        _timerRunningCoroutine = StartCoroutine(Timer());
        
        /*
         * Test functionalities
         */
        Debug.Log("Started request item: " + _requestedObject.Name);
        
        if(_requestedObject.Id == 1)
            ui.Beebread();
        
        if(_requestedObject.Id == 0)
            ui.Royaljelly();
        if(_requestedObject.Id == 5)
            ui.Water();
        
        _timerWaitingCoroutine = null;
    }
    
    public void BlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Disable(); 
    }

    public void UnlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Enable(); 
    } 
}
