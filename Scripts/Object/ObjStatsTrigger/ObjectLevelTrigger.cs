using UnityEngine;

public class ObjectLevelTrigger : ObjectStatTrigger
{
    public LevelData LevelData;
    //public TriggerLevelData TriggerLevelData;
    protected override void Start()
    {
        base.Start();
        LevelData.SetOri();
    }

    public override void RevertStatsData()
    {
        LevelData.Revert();
    }
    public override object CreateEffectData()
    {
        return LevelData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return LevelData.StatDataNameDataFlyweight.Name == name ? LevelData : null;
    }
}
