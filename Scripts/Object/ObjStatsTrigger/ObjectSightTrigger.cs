using UnityEngine;

public class ObjectSightTrigger : ObjectStatTrigger
{
    public SightData SightData;
    //public TriggerSightData TriggerSightData;
    protected override void Start()
    {
        base.Start();
        SightData.SetOri();
    }

    public override void RevertStatsData()
    {
        SightData.Revert();
    }
    public override object CreateEffectData()
    {
        return SightData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return SightData.StatDataNameDataFlyweight.Name == name ? SightData : null;
    }
}
