using System;
using System.Linq;
using UnityEngine;

public class ObjectStrengthTrigger : ObjectStatTrigger
{
    public StrengthData StrengthData;

    //public TriggerStrengthData TriggerStrengthData;

    protected override void Start()
    {
        base.Start();
        StrengthData.SetOri();
    }

    public override void RevertStatsData()
    {
        StrengthData.Revert();
    }
    public override object CreateEffectData()
    {
        return StrengthData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return StrengthData.StatDataNameDataFlyweight.Name == name ? StrengthData : null;
    }
}
