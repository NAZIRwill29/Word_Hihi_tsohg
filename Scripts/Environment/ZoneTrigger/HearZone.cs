using UnityEngine;

public class HearZone : StatusZone
{
    [SerializeField] protected HearDataFlyweight m_HearDataFlyweight;

    public override object CreateEffectData()
    {
        return m_HearDataFlyweight.HearData;
    }
}

