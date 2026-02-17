using System;
using UnityEngine;

public class FoodSpawnerTester : MonoBehaviour
{
    [SerializeField] private bool _sendSpawningRequest = false;
    
    private FoodSpawner _foodSpawner;

    private void Awake()
    {
        _foodSpawner = GetComponent<FoodSpawner>();
    }

    private void Update()
    {
        if (!_sendSpawningRequest) return;
        
        /*
        _foodSpawner.SpawnObject();
        */
        _sendSpawningRequest = false;
    }
}
