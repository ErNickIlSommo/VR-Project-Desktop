using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LarvaTester : MonoBehaviour
{
    [SerializeField] private bool _sendRequest = false;
    [SerializeField] GrabbableObjectData[] _grabbableObjects;

    private Larva _larva;

    private void Awake()
    {
        _larva = GetComponent<Larva>();
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
        
        Debug.Log("Send request for object with id: " + _grabbableObjects[index] );
    }
}
