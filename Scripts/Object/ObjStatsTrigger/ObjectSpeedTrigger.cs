using UnityEngine;

public class ObjectSpeedTrigger : ObjectStatTrigger
{
    public SpeedData SpeedData;
    //public TriggerSpeedData TriggerSpeedData;
    protected override void Start()
    {
        base.Start();
        SpeedData.SetOri();
    }

    public override void RevertStatsData()
    {
        SpeedData.Revert();
    }
    public override object CreateEffectData()
    {
        return SpeedData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return SpeedData.StatDataNameDataFlyweight.Name == name ? SpeedData : null;
    }
}
