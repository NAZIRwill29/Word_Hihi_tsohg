using UnityEngine;
using DG.Tweening;

public class UIShakerDOTween : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float strength = 10f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    [SerializeField] private bool fadeOut = true;

    private Tween currentTween;

    // public void ButtonShake(float customStrength = 1f)
    // {
    //     Shake(1, customStrength);
    // }

    public void Shake(RectTransform rectTransform, float? customDuration = null, float? customStrength = null)
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        currentTween = rectTransform.DOShakeAnchorPos(
            customDuration ?? duration,
            customStrength ?? strength,
            vibrato,
            randomness,
            snapping: false,
            fadeOut: fadeOut
        );
    }
}
