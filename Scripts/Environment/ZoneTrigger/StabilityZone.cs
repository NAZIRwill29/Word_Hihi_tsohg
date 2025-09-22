using UnityEngine;

public class StabilityZone : StatusZone
{
    [SerializeField] protected StabilityDataFlyweight m_StabilityDataFlyweight;

    public override object CreateEffectData()
    {
        return m_StabilityDataFlyweight.StabilityData;
    }
}
