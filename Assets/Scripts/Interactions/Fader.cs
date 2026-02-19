using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private ThirdPersonController playerController;
    
    public bool IsFading  { get; private set; }

    public IEnumerator FadeOut() => FadeTo(1f);
    public IEnumerator FadeIn() => FadeTo(0f);

    private IEnumerator FadeTo(float targetAlpha)
    {
        canvasGroup.interactable = true;
        IsFading = true;
        float startAlpha = canvasGroup.alpha;
        
        // Block input
        if (!playerController) yield return null;
        playerController.OnDisable();
        
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, k);
            yield return null;
        }
        
        canvasGroup.alpha = targetAlpha;
        
        // Unlock input
        playerController.OnEnable();
        
        IsFading = false;
        canvasGroup.interactable = true; 
    }
}
