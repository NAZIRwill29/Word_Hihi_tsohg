using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;

public class CameraZoomDOTween : CameraZoom
{
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private Ease zoomEase = Ease.InOutSine;
    private Tween currentTween;

    public override void ChangeZoom(float targetZoom)
    {
        m_ZoomSizeCurrent = targetZoom;

        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        currentTween = DOTween.To(
            () => m_CinemachineCamera.Lens.OrthographicSize,
            x => m_CinemachineCamera.Lens.OrthographicSize = x,
            m_ZoomSizeCurrent,
            zoomDuration
        ).SetEase(zoomEase);
    }

    public override void ZoomInOut(float zoomAddNum, float timeInterval, int repetition)
    {
        CancelZoom(false); // cancel any running tween

        float zoomInValue = m_ZoomSizeCurrent - zoomAddNum;
        float zoomOutValue = m_ZoomSizeCurrent + zoomAddNum;

        Sequence zoomSequence = DOTween.Sequence();
        for (int i = 0; i < repetition; i++)
        {
            zoomSequence.Append(
                DOTween.To(
                    () => m_CinemachineCamera.Lens.OrthographicSize,
                    x => m_CinemachineCamera.Lens.OrthographicSize = x,
                    zoomInValue,
                    zoomDuration
                ).SetEase(zoomEase)
            );

            zoomSequence.AppendInterval(timeInterval);

            zoomSequence.Append(
                DOTween.To(
                    () => m_CinemachineCamera.Lens.OrthographicSize,
                    x => m_CinemachineCamera.Lens.OrthographicSize = x,
                    zoomOutValue,
                    zoomDuration
                ).SetEase(zoomEase)
            );

            zoomSequence.AppendInterval(timeInterval);
        }

        // Reset to original zoom after sequence completes
        zoomSequence.Append(
            DOTween.To(
                () => m_CinemachineCamera.Lens.OrthographicSize,
                x => m_CinemachineCamera.Lens.OrthographicSize = x,
                m_ZoomSizeCurrent,
                zoomDuration
            ).SetEase(zoomEase)
        );

        currentTween = zoomSequence;
    }

    public override void CancelZoom(bool resetToOriginal)
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            currentTween = null;
        }

        if (resetToOriginal)
            m_CinemachineCamera.Lens.OrthographicSize = m_ZoomSizeOri;
    }
}
