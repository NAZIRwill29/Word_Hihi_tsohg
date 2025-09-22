using UnityEngine;

public class ObjectHearTrigger : ObjectStatTrigger
{
    public HearData HearData;
    //public TriggerHearData TriggerHearData;
    protected override void Start()
    {
        base.Start();
        HearData.SetOri();
    }

    public override void RevertStatsData()
    {
        HearData.Revert();
    }
    public override object CreateEffectData()
    {
        return HearData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return HearData.StatDataNameDataFlyweight.Name == name ? HearData : null;
    }
}
