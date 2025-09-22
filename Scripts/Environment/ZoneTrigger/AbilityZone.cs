using UnityEngine;

public class AbilityZone : StatusZone
{
    [SerializeField] protected AbilityDataFlyweight m_AbilityDataFlyweight;

    public override object CreateEffectData()
    {
        return m_AbilityDataFlyweight.AbilityData;
    }
}
