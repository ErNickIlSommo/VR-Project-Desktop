using System;
using UnityEngine;

public class NewNurseTester : MonoBehaviour
{
    [SerializeField] private bool _isActivityEnabled;
    private bool _guard = false;
    [SerializeField] private Activity _activity;

    private void Awake()
    {
        _activity = GetComponent<Activity>();
    }

    void Update()
    {
        if (_guard) return;
        if (!_isActivityEnabled) return;
        _activity.EnableActivity();
        _guard = false;
    }
}
