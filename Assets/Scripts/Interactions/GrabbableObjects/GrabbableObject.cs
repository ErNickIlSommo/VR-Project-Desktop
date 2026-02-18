using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    // in case the object has a RigidBody
    private Rigidbody _rb;
    
    [SerializeField] private GrabbableObjectData _objectData;
    public GrabbableObjectData ObjectData => _objectData;

    [SerializeField] private bool _isDropped = true;
    public bool IsDropped  => _isDropped;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public bool Interact(Interactor interactor)
    {
        // Check if interactor is holding another object
        if (IsHoldingOther(interactor)) return false;
        
        // Check if interactor is holding this object; in this case Drop it
        if (IsGrabbed(interactor))
        {
            Drop(interactor);
            return true;
        }
       
        // Grab the object
        Grab(interactor);
        return true;
    }

    private void Grab(Interactor interactor)
    {
        if (_rb)
        {
            _rb.isKinematic = true;
        }
        
        BlockMovement(interactor);
        
        _isDropped = false;
        interactor.PlayerInteractionStatus.SetGrabbedObject(this);
        transform.SetParent(interactor.PlayerInteractionStatus.GrabbedSpotPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        UnlockMovement(interactor);
        
        // Debug.Log("Grabbed");
    }

    private void Drop(Interactor interactor)
    {
        if (_rb)
        {
            _rb.isKinematic = false;
        } 

        BlockMovement(interactor); 
    
        _isDropped = true; 
        interactor.PlayerInteractionStatus.SetGrabbedObject();
        transform.SetParent(null, true);
        
        UnlockMovement(interactor);
        
        // Debug.Log("Dropped");
    }

    private bool IsHoldingOther(Interactor interactor)
    {
        // If interactor has no Transform for the grabbed object, just return false
        if(!interactor.PlayerInteractionStatus.GrabbedObjectTransform) return false;
        
        // if interactor has already grabbed another object
        return interactor.PlayerInteractionStatus.HasGrabbed && 
               transform.name != interactor.PlayerInteractionStatus.GrabbedObjectTransform.name;
    }
    
    private bool IsGrabbed(Interactor interactor)
    {
        // If interactor has no Transform for the grabbed object, just return false
        if(!interactor.PlayerInteractionStatus.GrabbedObjectTransform) return false;
        
        return interactor.PlayerInteractionStatus.HasGrabbed && 
               transform.name == interactor.PlayerInteractionStatus.GrabbedObjectTransform.name;
    }

    public void ForceGrab(Interactor interactor)
    {
        Grab(interactor);
    }

    public void ForceDrop(Interactor interactor)
    {
        Drop(interactor);
    }
    
    public void ForceDropAndDestroy(Interactor interactor)
    {
        Drop(interactor);
        Destroy(transform.gameObject);
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
