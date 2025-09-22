using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public LightingRunTimeSetSO LightingRunTimeSetSO;
    public LightingRunTimeSetSO HideLightingRunTimeSetSO;
    public LightingRunTimeSetSO PlayerLightingRunTimeSetSO;
    public Animator Animator;
    [SerializeField] private List<NameDataFlyweight> GlobalLightNames;
    public StabilityStateManager GhostStabilityStateManager;

    void OnEnable()
    {
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.AddListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.AddListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.AddListener(OnGhostLowStability);
        }
    }

    void OnDisable()
    {
        if (GhostStabilityStateManager)
        {
            GhostStabilityStateManager.OnLowStability.RemoveListener(OnGhostLowStability);
            GhostStabilityStateManager.OnHighStability.RemoveListener(OnGhostHighStability);
            GhostStabilityStateManager.OnZeroStability.RemoveListener(OnGhostLowStability);
        }
    }

    public void AllLightOn(bool isLight)
    {
        foreach (var item in LightingRunTimeSetSO.Items)
        {
            if (item is ObjectLight objectLight)
                objectLight.LightOn(isLight);
        }

        foreach (var item in PlayerLightingRunTimeSetSO.Items)
        {
            if (item is ObjectLight objectLight)
                objectLight.LightOn(isLight);
        }
    }

    public void ChangeGlobalLight(string name)
    {
        int index = GlobalLightNames.FindIndex(x => x.Name == name);
        Animator.SetInteger("State", index);
    }

    #region Stability
    private void OnGhostHighStability(string name)
    {
        ChangeGlobalLight("Night");
    }
    private void OnGhostLowStability(string name)
    {
        ChangeGlobalLight("Scary");
    }
    #endregion

    public void TestButton()
    {
        OnGhostLowStability("angry");
    }
    public void TestButton2()
    {
        OnGhostLowStability("");
    }
}
