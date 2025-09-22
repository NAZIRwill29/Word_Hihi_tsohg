using UnityEngine;

public class ObjectVisibilityTrigger : ObjectStatTrigger
{
    public VisibilityData VisibilityData;
    //public TriggerVisibilityData TriggerVisibilityData;
    protected override void Start()
    {
        base.Start();
        VisibilityData.SetOri();
    }

    public override void RevertStatsData()
    {
        VisibilityData.Revert();
    }
    public override object CreateEffectData()
    {
        return VisibilityData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return VisibilityData.StatDataNameDataFlyweight.Name == name ? VisibilityData : null;
    }
}
