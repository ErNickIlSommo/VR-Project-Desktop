using System;
using UnityEngine;

public class TestGlobalData : MonoBehaviour
{
    [SerializeField] private bool test = false;

    [SerializeField] private InteriorMaster interiorMaster;

    private bool _previous;

    private void Awake()
    {
        interiorMaster = GetComponent<InteriorMaster>();
    }

    private void Update()
    {
        if (_previous != test)
            interiorMaster.TestGlobal = test;
    }
}
