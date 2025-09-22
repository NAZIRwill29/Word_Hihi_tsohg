using UnityEngine;

public class ObjectStaminaTrigger : ObjectStatTrigger
{
    public StaminaData StaminaData;
    //public TriggerStaminaData TriggerStaminaData;
    protected override void Start()
    {
        base.Start();
        StaminaData.SetOri();
    }

    public override void RevertStatsData()
    {
        StaminaData.Revert();
    }
    public override object CreateEffectData()
    {
        return StaminaData;
    }
    public override StatsData GetStatsDataByName(string name)
    {
        return StaminaData.StatDataNameDataFlyweight.Name == name ? StaminaData : null;
    }
}
