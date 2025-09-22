using UnityEngine;

public class ObjectShootableTrigger : ObjectStatTrigger
{
    public ShootData ShootData;
    //public TriggerShootData TriggerShootData;
    protected override void Start()
    {
        base.Start();
        ShootData.SetOri();
    }

    public override void RevertStatsData()
    {
        ShootData.Revert();
    }
    public override object CreateEffectData()
    {
        return ShootData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return ShootData.StatDataNameDataFlyweight.Name == name ? ShootData : null;
    }
}

