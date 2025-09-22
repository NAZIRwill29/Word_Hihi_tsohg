using UnityEngine;

public class ObjectStabilityTrigger : ObjectStatTrigger
{
    public StabilityData StabilityData;
    //public TriggerStaminaData TriggerStaminaData;
    protected override void Start()
    {
        base.Start();
        StabilityData.SetOri();
    }

    public override void RevertStatsData()
    {
        StabilityData.Revert();
    }
    public override object CreateEffectData()
    {
        return StabilityData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return StabilityData.StatDataNameDataFlyweight.Name == name ? StabilityData : null;
    }
}
