using System;
using UnityEngine;

public class PlayerInteractionStatus : MonoBehaviour
{
    [SerializeField] private bool _hasGrabbed = false;
    public bool HasGrabbed { get => _hasGrabbed; set => _hasGrabbed = value; }

    [SerializeField] private Transform _grabbedSpotPoint;
    public Transform GrabbedSpotPoint => _grabbedSpotPoint;

    [SerializeField] private Transform _grabbedObjectTransform;
    public Transform GrabbedObjectTransform => _grabbedObjectTransform;
    
    [SerializeField]  private GameObject _grabbedObject;
    public GameObject GrabbedObject => _grabbedObject ;
    
    [SerializeField] private GrabbableObject _grabbableObject;
    public GrabbableObject GrabbableObject => _grabbableObject;

    private GrabbableObjectData _objectData;
    public GrabbableObjectData ObjectData => _objectData;

    private PlayerAnimationController _animationController;
    
    private CharacterController _characterController;
    
    public CharacterController CharacterController => _characterController;

    private void Awake()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        _characterController = GetComponent<CharacterController>();
    }

    public void SetGrabbedObject()
    {
        _grabbedObjectTransform = null;
        _grabbableObject = null;
        _objectData = null;
        _grabbedObject = null;
        _hasGrabbed = false;
        _animationController.Drop();
    }

    public void SetGrabbedObject(GrabbableObject grabbableObject)
    {
        _grabbedObjectTransform = grabbableObject.transform;
        _objectData = grabbableObject.ObjectData;
        _grabbedObject = grabbableObject.gameObject;
        _grabbableObject = grabbableObject;
        _hasGrabbed = !grabbableObject.IsDropped;
        _animationController.Grab();
    }

    public void SetGrabbedObject(Transform grabbedTransform, GrabbableObjectData data, GrabbableObject grabbableObject, bool isGrabbing)
    {
        _grabbedObjectTransform = grabbedTransform;
        _objectData = data;
        _grabbableObject = grabbableObject;
        _hasGrabbed = isGrabbing;
    }
}
