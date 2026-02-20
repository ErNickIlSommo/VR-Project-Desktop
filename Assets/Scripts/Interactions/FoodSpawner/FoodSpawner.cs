using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class FoodSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] GrabbableObjectData _grabbableObjectData;
    public GrabbableObjectData GrabbableObjectData => _grabbableObjectData;
    
    [SerializeField] private float _cooldown;
    public float Cooldown { get => _cooldown; set => _cooldown = value; }

    [SerializeField] private GameObject _content;

    [SerializeField] private float _timer;

    [SerializeField] private bool _isInCooldown = false;
    private bool _canInteract = true;

    private void Awake()
    {
        _cooldown = _grabbableObjectData.Cooldown;
    }

    public bool Interact(Interactor interactor)
    {
        // Debug.Log("Start Interaction with spawner");
        if (!_canInteract) return false;
        if (_isInCooldown) return false;
        if (interactor.PlayerInteractionStatus.HasGrabbed) return false;
        
        BlockMovement(interactor);
        SpawnObject(interactor);
        UnlockMovement(interactor);
        
        return true;
    }
    
    public void EnableInteraction()
    {
        _canInteract = true;
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }
    
    private void SpawnObject(Interactor interactor)
    {
        GameObject spawnedObject = Instantiate(_grabbableObjectData.Object, interactor.PlayerInteractionStatus.GrabbedSpotPoint);
        
        GrabbableObject grabbableObject = spawnedObject.GetComponent<GrabbableObject>();
        
        grabbableObject.ForceGrab(interactor);
        
        StartCoroutine(Timer());
        _isInCooldown = true;
        _content.SetActive(false);
    }

    private IEnumerator Timer()
    {
        _timer = 0f;
        
        while (_timer < _cooldown)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        
        _isInCooldown = false;
        _content.SetActive(true);
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
