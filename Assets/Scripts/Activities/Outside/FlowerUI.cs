using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image targetImage;
    [SerializeField] private FlowerImages flowerImages;

    private Dictionary<FlowerState, Sprite> _state;

    private void Awake()
    {
        _state = new Dictionary<FlowerState, Sprite>(flowerImages.entries.Length);
        foreach (var e in flowerImages.entries)
            _state[e.flowerState] = e.sprite;

        Hide();
    }

    public void Show()
    {
        group.alpha = 1f;
    }

    public void Hide()
    {
        group.alpha = 0f;
    }

    public void UpdateImage(FlowerState state)
    {
        if (_state.TryGetValue(state, out var sprite))
            targetImage.sprite = sprite;
        targetImage.enabled = sprite != null;
    }
}
