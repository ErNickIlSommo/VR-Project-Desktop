using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BillboardUI : MonoBehaviour
{
    private Camera targetCamera;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        
    }

    void LateUpdate()
    {
        if (!targetCamera)
        {
            // Debug.Log("Ciao");
            return;
        }

        Vector3 dir = transform.position - targetCamera.transform.position;

        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
