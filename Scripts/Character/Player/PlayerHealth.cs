using UnityEngine;

//TODO() - SHAKE + EXIT STRUGGLE MODE - MADE NAN IN COMBAT DISTANCE IMG
public class PlayerHealth : ObjectHealth
{
    [SerializeField] private float m_CameraShakeIntensity = 1;
    public float CameraShakeIntensity { get; set; }

    protected override void Start()
    {
        base.Start();
        CameraShakeIntensity = m_CameraShakeIntensity;
    }

    public override void TakeDamage(HealthData healthData)
    {
        if (!m_ObjectT.IsAlive || IsInvulnerable) return;
        base.TakeDamage(healthData);

        Debug.Log("TakeDamage");

        if (!m_ObjectT.InBattle)
            CameraManager.Instance.CameraShake.Shake(CameraShakeIntensity);
        // else
        //     GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }

    public void ResetCameraShakeIntensity()
    {
        CameraShakeIntensity = m_CameraShakeIntensity;
    }
}
