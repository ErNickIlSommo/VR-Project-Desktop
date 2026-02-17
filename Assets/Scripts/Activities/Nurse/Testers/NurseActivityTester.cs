using System;
using UnityEngine;

public class NurseActivityTester : MonoBehaviour
{
    [SerializeField] private bool startActivity;
    [SerializeField] private int howMany = 5000;

    private NurseActivity _nurseActivity;

    private void Awake()
    {
        _nurseActivity = GetComponent<NurseActivity>();
    }

    private void Update()
    {
        if (_nurseActivity.IsActivityStarted) return;
        if (!startActivity) return;
        
        startActivity = false;
        _nurseActivity.StartActivity();
    }
}
