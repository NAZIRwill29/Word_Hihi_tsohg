using UnityEngine;

public class SightZone : StatusZone
{
    [SerializeField] protected SightDataFlyweight m_SightDataFlyweight;

    public override object CreateEffectData()
    {
        return m_SightDataFlyweight.SightData;
    }
}
