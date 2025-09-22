using UnityEngine;

public class SpeedZone : StatusZone
{
    [SerializeField] protected SpeedDataFlyweight m_SpeedDataFlyweight;

    public override object CreateEffectData()
    {
        return m_SpeedDataFlyweight.SpeedData;
    }
}

