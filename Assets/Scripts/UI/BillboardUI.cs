using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BillboardUI : MonoBehaviour
{
    private Camera targetCamera;

    [SerializeField] private Image cloud;
    [SerializeField] private Image request;
    [SerializeField] private Image ok;
    [SerializeField] private Image nope;
    [SerializeField] private Image beebread;
    [SerializeField] private Image royaljelly;
    [SerializeField] private Image water;

    
    
    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        
        cloud.enabled = false;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = false;
    }

    public void Restart()
    {
        cloud.enabled = false;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = false; 
    }

    public void OnRequest()
    {
        cloud.enabled = true;
        request.enabled = true;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = false;
    }

    public void OnNope()
    {
        cloud.enabled = true;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = true;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = false;
    }

    public void Ok()
    {
        cloud.enabled = true;
        request.enabled = false;
        ok.enabled = true;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = false;
    }

    public void Beebread()
    {
        cloud.enabled = true;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = true;
        royaljelly.enabled = false;
        water.enabled = false;
    }

    public void Royaljelly()
    {
        cloud.enabled = true;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = true;
        water.enabled = false;
    }

    public void Water()
    {
        cloud.enabled = true;
        request.enabled = false;
        ok.enabled = false;
        nope.enabled = false;
        beebread.enabled = false;
        royaljelly.enabled = false;
        water.enabled = true;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 dir = transform.position - targetCamera.transform.position;

        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
