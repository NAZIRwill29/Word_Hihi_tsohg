using UnityEngine;

public class MoneyZone : StatusZone
{
    [SerializeField] protected MoneyDataFlyweight m_MoneyDataFlyweight;

    public override object CreateEffectData()
    {
        return m_MoneyDataFlyweight.MoneyData;
    }
}
