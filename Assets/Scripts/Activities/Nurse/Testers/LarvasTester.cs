using System;
using UnityEngine;

public class LarvasTester : MonoBehaviour
{
    [SerializeField] private bool shuffle;

    private Larvas _larvas;

    private void Awake()
    {
        _larvas = GetComponent<Larvas>();
    }

    private void Update()
    {
        if (!shuffle) return;
        
        _larvas.InitLarvasManager();
        shuffle = false;
    }
}
