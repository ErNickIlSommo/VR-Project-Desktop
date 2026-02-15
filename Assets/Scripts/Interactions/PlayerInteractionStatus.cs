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

    public void SetGrabbedObject(Transform grabbedTransform, bool isGrabbing)
    {
        _grabbedObjectTransform = grabbedTransform;
        _hasGrabbed = isGrabbing;
    }
}
