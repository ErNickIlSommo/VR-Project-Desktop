using System;
using UnityEngine;

public class GeneralDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;

    protected void Awake()
    {
        _canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            _canvas.SetActive(false);
    }
}
