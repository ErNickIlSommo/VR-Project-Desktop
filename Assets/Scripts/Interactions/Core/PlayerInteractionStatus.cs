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

    private GrabbableObjectData _objectData;
    public GrabbableObjectData ObjectData => _objectData;

    public void SetGrabbedObject(Transform grabbedTransform, GrabbableObjectData data, bool isGrabbing)
    {
        _grabbedObjectTransform = grabbedTransform;
        _objectData = data;
        _hasGrabbed = isGrabbing;
    }

    public void SetGrabbedObject()
    {
        _grabbedObjectTransform = null;
        _objectData = null;
        _hasGrabbed = false;
    }
}
