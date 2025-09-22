using UnityEngine;

public class ObjectMoneyTrigger : ObjectStatTrigger
{
    public MoneyData MoneyData;
    //public TriggerMoneyData TriggerMoneyData;
    protected override void Start()
    {
        base.Start();
        MoneyData.SetOri();
    }

    public override void RevertStatsData()
    {
        MoneyData.Revert();
    }
    public override object CreateEffectData()
    {
        return MoneyData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return MoneyData.StatDataNameDataFlyweight.Name == name ? MoneyData : null;
    }
}
