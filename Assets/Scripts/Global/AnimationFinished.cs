using UnityEngine;
using System;

public class AnimationFinished : MonoBehaviour
{
    public event Action Ended;

    // Chiamata dall'Animation Event
    public void OnAnimFinished()
    {
        Ended?.Invoke();
        Debug.Log("ANIMATION FINISHED: Animazione terminata");
    } 
}
