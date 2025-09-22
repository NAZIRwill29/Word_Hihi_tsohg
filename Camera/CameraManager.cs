using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CameraShake CameraShake;
    public CameraZoom CameraZoom;
    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    [SerializeField] private CinemachinePositionComposer m_CinemachinePositionComposer;
    [SerializeField] private Vector3 m_CameraOffsetOri;

    public void ResetCameraOffset()
    {
        ChangeCameraOffset(m_CameraOffsetOri);
    }

    public void ChangeCameraOffset(Vector3 offset)
    {
        m_CinemachinePositionComposer.TargetOffset = offset;
    }

    public void ChangeTargetCamera(Transform transform)
    {
        m_CinemachineCamera.LookAt = transform;
        m_CinemachineCamera.Follow = transform;
    }
}
