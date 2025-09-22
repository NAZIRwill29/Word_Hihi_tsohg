using UnityEngine;

public class ObjectMagicTrigger : ObjectStatTrigger
{
    public MagicData MagicData;
    //public TriggerMagicData TriggerMagicData;
    protected override void Start()
    {
        base.Start();
        MagicData.SetOri();
    }

    public override void RevertStatsData()
    {
        MagicData.Revert();
    }
    public override object CreateEffectData()
    {
        return MagicData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return MagicData.StatDataNameDataFlyweight.Name == name ? MagicData : null;
    }
}
