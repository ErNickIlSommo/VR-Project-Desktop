using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI: MonoBehaviour
{
    private Camera targetCamera;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image targetImage;
    [SerializeField] private LarvaImages larvaImages;
    
    private Dictionary<LarvaSituation, Sprite> _food;

    private void Awake()
    {
        if(targetCamera == null)
            targetCamera = Camera.main;
        
        _food = new Dictionary<LarvaSituation, Sprite>(larvaImages.entries.Length);
        foreach (var e in larvaImages.entries)
            _food[e.larvaSituation] = e.sprite;
        
        Hide();
    }

    public void Show()
    {
        group.alpha = 1f;
        // group.interactable = true;
        // group.blocksRaycasts = true;
    }

    public void Hide()
    {
        group.alpha = 0f;
        // group.interactable = true;
        // group.blocksRaycasts = true; 
    }

    public void UpdateImage(LarvaSituation situation)
    {
        if(_food.TryGetValue(situation, out var sprite))
            targetImage.sprite = sprite;
        targetImage.enabled = sprite  != null;
    }
    
    void LateUpdate()
    {
        if (!targetCamera) return;

        Vector3 dir = transform.position - targetCamera.transform.position;

        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}