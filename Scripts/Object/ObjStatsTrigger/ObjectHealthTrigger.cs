using System;
using System.Linq;
using UnityEngine;

public class ObjectHealthTrigger : ObjectStatTrigger
{
    public HealthData HealthData;
    //protected HealthData m_HealthDataOrigin = new();
    [SerializeField] private ItemStrength m_ItemStrength;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_ItemStrength.OnStrengthChange += OnStrengthChange;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_ItemStrength.OnStrengthChange -= OnStrengthChange;
    }

    protected override void Start()
    {
        base.Start();
        HealthData.SetOri();
    }

    public override void RevertStatsData()
    {
        HealthData.Revert();
    }

    public override object CreateEffectData()
    {
        return HealthData;
    }

    private void OnStrengthChange(float strengthNum)
    {
        if (VariableFinder.TryGetVariableContainNameFromList(HealthData.DataNumVars, "Health", out DataNumericalVariable dataNumerical))
            dataNumerical.AddNumVariable = -strengthNum;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return HealthData.StatDataNameDataFlyweight.Name == name ? HealthData : null;
    }
}
