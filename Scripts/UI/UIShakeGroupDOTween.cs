using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIShakeGroupDOTween : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float strength = 10f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    [SerializeField] private bool fadeOut = true;

    [Header("Target UI Elements")]
    [SerializeField] private List<RectTransform> targets;

    private List<Tween> activeTweens = new List<Tween>();

    public void ShakeAll(float customStrength = 1)
    {
        ShakeAll(1, customStrength);
    }

    public void ShakeAll(float? customDuration = null, float? customStrength = null)
    {
        StopAll();

        float shakeDuration = customDuration ?? duration;
        float shakeStrength = customStrength ?? strength;

        foreach (RectTransform target in targets)
        {
            if (target == null) continue;

            Tween t = target.DOShakeAnchorPos(
                shakeDuration,
                shakeStrength,
                vibrato,
                randomness,
                snapping: false,
                fadeOut: fadeOut
            );

            activeTweens.Add(t);
        }
    }

    public void StopAll()
    {
        foreach (Tween t in activeTweens)
        {
            if (t.IsActive()) t.Kill();
        }
        activeTweens.Clear();
    }

    // Optional: automatically populate all RectTransform children
    [ContextMenu("Auto-Populate Targets From Children")]
    private void AutoPopulateTargets()
    {
        targets.Clear();
        RectTransform[] children = GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rect in children)
        {
            if (rect != (RectTransform)transform) // exclude self
                targets.Add(rect);
        }
    }
}
