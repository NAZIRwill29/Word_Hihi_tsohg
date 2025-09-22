using System;
using UnityEngine;

public class StabilityStatDrainOverTime : StatDrainOverTime
{
    //[SerializeField] private StabilityData m_StabilityData;
    public ObjectStability ObjectStability;
    [SerializeField] private float m_AfterLowHealthDrainTime = 1;
    public HealthStateManager HealthStateManager;

    void OnEnable()
    {
        if (!HealthStateManager) return;
        HealthStateManager.OnLowHealth.AddListener(OnLowHealth);
        HealthStateManager.OnHighHealth.AddListener(OnHighHealth);
    }

    void OnDisable()
    {
        if (!HealthStateManager) return;
        HealthStateManager.OnLowHealth.RemoveListener(OnLowHealth);
        HealthStateManager.OnHighHealth.RemoveListener(OnHighHealth);
    }

    // public override void Activate(bool isTrue)
    // {
    //     base.Activate(isTrue);
    //     if (m_StatsMicrobarDataFlyWeight is StabilityDataFlyweight stabilityDataFlyweight)
    //         ChangeDrainVariable(stabilityDataFlyweight.StabilityData);
    // }

    protected override void DrainStat()
    {
        if (ObjectStability != null && ObjectStability is IDrain drain && m_StatsMicrobarDataFlyWeight is StabilityDataFlyweight stabilityDataFlyweight)
            drain.DrainStat(stabilityDataFlyweight.StabilityData);
    }

    private void OnHighHealth()
    {
        ResetDrainCooldown();
        // if (m_StatsMicrobarDataFlyWeight is StabilityDataFlyweight stabilityDataFlyweight)
        //     ChangeDrainVariable(stabilityDataFlyweight.StabilityData);
    }

    private void OnLowHealth()
    {
        // DataNumericalVariable change = VariableFinder.GetVariableContainNameFromList(m_StabilityData.DataNumVars, "Stability");
        // change.AddNumVariable = m_AfterLowHealthDrainNum;
        DrainTime = m_AfterLowHealthDrainTime;
    }
}
