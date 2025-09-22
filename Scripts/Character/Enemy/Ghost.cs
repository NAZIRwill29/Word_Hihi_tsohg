using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//follow projectile - objectpool for random
public class Ghost : Enemy2D
{
    public GhostTemplate GhostTemplate;
    public GhostCombatDataFlyweight GhostCombatDataFlyweight;
    public MircobarSystem HealthMircobarSystem, StabilityMircobarSystem, ManaMircobarSystem;
    public Light2D[] Light2DEyes;
    public StabilityStateManager GhostStabilityStateManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.AddListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.AddListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.AddListener(OnGhostLowStability);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.RemoveListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.RemoveListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.RemoveListener(OnGhostLowStability);
        }
    }

    private void OnGhostHighStability(string name)
    {
        foreach (var item in Light2DEyes)
        {
            item.enabled = true;
        }
    }
    private void OnGhostLowStability(string name)
    {
        foreach (var item in Light2DEyes)
        {
            item.enabled = false;
        }
    }

    public void TestButton()
    {
        OnGhostLowStability("");
    }
    public void TestButton2()
    {
        OnGhostLowStability("");
    }
}
