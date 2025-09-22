using UnityEngine;

public class ObjectAbilityTrigger : ObjectStatTrigger
{
    public AbilityData AbilityData;
    protected override void Start()
    {
        base.Start();
        AbilityData.SetOri();
    }

    public override void RevertStatsData()
    {
        AbilityData.Revert();
    }
    public override object CreateEffectData()
    {
        return AbilityData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return AbilityData.StatDataNameDataFlyweight.Name == name ? AbilityData : null;
    }
}
