using UnityEngine;

public class ObjectDefenseTrigger : ObjectStatTrigger
{
    public DefenseData DefenseData;
    protected override void Start()
    {
        base.Start();
        DefenseData.SetOri();
    }

    public override void RevertStatsData()
    {
        DefenseData.Revert();
    }
    public override object CreateEffectData()
    {
        return DefenseData;
    }

    public override StatsData GetStatsDataByName(string name)
    {
        return DefenseData.StatDataNameDataFlyweight.Name == name ? DefenseData : null;
    }
}
