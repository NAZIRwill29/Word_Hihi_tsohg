using UnityEngine;

public class ObjectEvadeableTrigger : ObjectStatTrigger
{
    public EvadeData EvadeData;
    protected override void Start()
    {
        base.Start();
        EvadeData.SetOri();
    }

    public override void RevertStatsData()
    {
        EvadeData.Revert();
    }
    public override object CreateEffectData()
    {
        return EvadeData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return EvadeData.StatDataNameDataFlyweight.Name == name ? EvadeData : null;
    }
}