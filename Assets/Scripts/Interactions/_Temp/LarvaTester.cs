using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LarvaTester : MonoBehaviour
{
    [SerializeField] private bool _sendRequest = false;
    [SerializeField] GrabbableObjectData[] _grabbableObjects;

    private LarvaInteraction _larva;

    private void Awake()
    {
        _larva = GetComponent<LarvaInteraction>();
    }

    private void Update()
    {
        if (!_sendRequest) return;
        if (_larva.IsRequestRunning)
        {
            _sendRequest = false;
            return;
        }
        
        int index = Random.Range(0, _grabbableObjects.Length);
        var status = _larva.StartRequest(_grabbableObjects[index]);
        
        Debug.Log("Send request for object with name: " + _grabbableObjects[index].Name );
    }
}
