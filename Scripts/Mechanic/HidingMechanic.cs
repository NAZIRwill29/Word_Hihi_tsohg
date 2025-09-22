using Unity.VisualScripting;
using UnityEngine;

public class HidingMechanic : MonoBehaviour
{
    [SerializeField] private float m_HideZoomSize = 6;
    private ObjectLight m_ObjectLight;
    [SerializeField] private NormalUI m_NormalUI;
    [SerializeField] private CombatStarterCollision m_CombatStarterCollision;
    private bool m_IsInHide;
    public HidingPlace HidingPlace { get; set; }

    public void StartHide(HidingPlace hidingPlace, ObjectLight objectLight, Vector3 cameraOffset, Transform transform)
    {
        if (m_IsInHide) return;
        m_IsInHide = true;
        HidingPlace = hidingPlace;
        //zoom in to hide place
        CameraManager.Instance.CameraZoom.ChangeZoom(m_HideZoomSize);
        CameraManager.Instance.ChangeCameraOffset(cameraOffset);
        CameraManager.Instance.ChangeTargetCamera(transform);
        m_NormalUI.ChangeTargetUIFollow(transform);
        //hide player
        GameManager.Instance.player2D.Animator.SetBool("Hide", true);
        GameManager.Instance.player2D.ObjectVisibility.VisibilityChange(false, false);
        m_CombatStarterCollision.IsActive = false;
        //other obj and bg make darker
        GameManager.Instance.LightManager.AllLightOn(false);
        // obj brighter
        m_ObjectLight = objectLight;
        m_ObjectLight.LightOn(true);
        //change input in input system - hidingInputManagerSO
        GameManager.Instance.ChangePlayMode("MiniGame");
        //stop player move
        GameManager.Instance.player2D.ObjectSpeed.CanWalkChange(false);
        // make sound graphics effect for ghost
        GameManager.Instance.EnemyManager.SetAllAnimation("PlayerHide", true);
    }

    public void UnHide()
    {
        if (!m_IsInHide) return;
        m_IsInHide = false;

        CameraManager.Instance.CameraZoom.CancelZoom(true);
        CameraManager.Instance.ResetCameraOffset();
        CameraManager.Instance.ChangeTargetCamera(GameManager.Instance.player2D.ObjTransform);
        m_NormalUI.ChangeTargetUIFollow(GameManager.Instance.player2D.ObjTransform);

        GameManager.Instance.player2D.Animator.SetBool("Hide", false);
        GameManager.Instance.player2D.ObjectVisibility.VisibilityChange(true, true);

        m_CombatStarterCollision.IsActive = true;

        GameManager.Instance.LightManager.AllLightOn(true);
        m_ObjectLight.LightOn(false);

        GameManager.Instance.ChangePlayMode(GameManager.Instance.PlayModeManager.NormalListenerNameData.Name);

        GameManager.Instance.player2D.ObjectSpeed.CanWalkChange(true);
        GameManager.Instance.EnemyManager.SetAllAnimation("PlayerHide", false);
        Debug.Log("UnHide");
    }

    public void DiscoverableHide()
    {
        GameManager.Instance.player2D.ObjectVisibility.VisibilityChange(true, true);
        m_CombatStarterCollision.IsActive = true;
    }
}
