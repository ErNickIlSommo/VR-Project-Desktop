using System;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger: " + other.gameObject.name);
        if (!other.CompareTag("Player")) return;
        canvasGroup.alpha = 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        canvasGroup.alpha = 0;
    }
}
