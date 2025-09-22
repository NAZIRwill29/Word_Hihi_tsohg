using UnityEngine;

public class ObjectMeleableTrigger : ObjectStatTrigger
{
    public MeleeData MeleeData;
    //public TriggerMeleeData TriggerMeleeData;

    protected override void Start()
    {
        base.Start();
        MeleeData.SetOri();
    }

    public override void RevertStatsData()
    {
        MeleeData.Revert();
    }
    public override object CreateEffectData()
    {
        return MeleeData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return MeleeData.StatDataNameDataFlyweight.Name == name ? MeleeData : null;
    }
}
