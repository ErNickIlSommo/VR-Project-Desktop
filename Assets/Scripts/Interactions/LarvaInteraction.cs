using UnityEngine;
using System.Collections;

public class LarvaInteraction : MonoBehaviour, IInteractable
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
        
        Debug.Log("Started request item with id: " + _requestedObject.Id);
        
        return true;
    }
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Larva Interaction started");
        
        if (!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        
        Debug.Log("Player is grabbing something"); 
        
        if (!IsCorrectIngredient(interactor))
        {
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
        
        Debug.Log("Task failed");
    }

    private void EatIngredient()
    {
        _requestedObject = null;
        _timerRunningCoroutine = null;
        _isRequestRunning = false;
        
        Debug.Log("Task succeded");
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
