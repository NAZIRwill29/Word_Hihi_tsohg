using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] protected CinemachineCamera m_CinemachineCamera;
    [SerializeField] protected float m_ZoomSizeOri = 12;
    protected float m_ZoomSizeCurrent;

    protected virtual void Start()
    {
        m_CinemachineCamera.Lens.OrthographicSize = m_ZoomSizeOri;
        m_ZoomSizeCurrent = m_ZoomSizeOri;
    }

    public virtual void ChangeZoom(float num)
    {
        m_ZoomSizeCurrent = num;
        m_CinemachineCamera.Lens.OrthographicSize = m_ZoomSizeCurrent;
    }

    public virtual void ZoomInOut(float zoomAddNum, float timeInterval, int repetition)
    {
        StartCoroutine(ZoomInOutCoroutine(zoomAddNum, timeInterval, repetition));
    }

    private IEnumerator ZoomInOutCoroutine(float zoomAddNum, float timeInterval, int repetition)
    {
        for (int i = 0; i < repetition; i++)
        {
            ChangeZoom(m_ZoomSizeCurrent - zoomAddNum);
            yield return new WaitForSeconds(timeInterval);
            ChangeZoom(m_ZoomSizeCurrent + zoomAddNum);
        }
        ChangeZoom(m_ZoomSizeCurrent);
    }

    public virtual void CancelZoom(bool resetToOriginal)
    {
        StopCoroutine(ZoomInOutCoroutine(0, 0, 0));

        if (resetToOriginal)
            m_CinemachineCamera.Lens.OrthographicSize = m_ZoomSizeOri;
    }
}
