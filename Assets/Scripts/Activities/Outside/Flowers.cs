using System;
using UnityEngine;
using System.Collections.Generic;

public class Flowers : MonoBehaviour
{
    public Action<bool> OnActivityRunning;
    public Action<bool> OnActivityFinished;
    
    [SerializeField] private List<Flower> flowers;

    [SerializeField] private int counter;
    
    [SerializeField] private int _numFlowers;

    private void Awake()
    {
        counter = 0;
        foreach (Flower flower in transform.GetComponentsInChildren<Flower>() )
        {
            flowers.Add(flower);
            flower.OnInteractionFinished += HandleInteractionFinished;
            flower.FlowerObject.SetActive(false);
        }
        _numFlowers = flowers.Count;
    }

    public void StartActivity()
    {
        foreach (Flower flower in flowers)
        {
            flower.FlowerObject.SetActive(true);
        }
    }

    private void HandleInteractionFinished(bool status)
    {
        if (!status) return;
        
        counter++;
        if (counter < _numFlowers)
        {
            if(OnActivityRunning != null) OnActivityRunning.Invoke(true);
            return;
        }

        ApplicationFinished();
    }

    private void ApplicationFinished()
    {
        if (OnActivityFinished != null) OnActivityFinished.Invoke(true);
    }
}
