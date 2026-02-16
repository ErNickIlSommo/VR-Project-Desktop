using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IInteractable
{
    // in case the object has a RigidBody
    private Rigidbody _rb;
    private Transform _transform;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
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
        
        interactor.PlayerInteractionStatus.SetGrabbedObject(_transform, true);
        _transform.SetParent(interactor.PlayerInteractionStatus.GrabbedSpotPoint, false);
        _transform.localPosition = Vector3.zero;
        _transform.localRotation = Quaternion.identity;
        
        // Debug.Log("Grabbed");
    }

    private void Drop(Interactor interactor)
    {
        if (_rb)
        {
            _rb.isKinematic = false;
        } 
        
        interactor.PlayerInteractionStatus.SetGrabbedObject(null, false);
        _transform.SetParent(null, true);
        // Debug.Log("Dropped");
    }

    private bool IsHoldingOther(Interactor interactor)
    {
        // If interactor has no Transform for the grabbed object, just return false
        if(!interactor.PlayerInteractionStatus.GrabbedObjectTransform) return false;
        
        // if interactor has already grabbed another object
        return interactor.PlayerInteractionStatus.HasGrabbed && 
               _transform.name != interactor.PlayerInteractionStatus.GrabbedObjectTransform.name;
    }
    
    private bool IsGrabbed(Interactor interactor)
    {
        // If interactor has no Transform for the grabbed object, just return false
        if(!interactor.PlayerInteractionStatus.GrabbedObjectTransform) return false;
        
        return interactor.PlayerInteractionStatus.HasGrabbed && 
               _transform.name == interactor.PlayerInteractionStatus.GrabbedObjectTransform.name;
    }
}
