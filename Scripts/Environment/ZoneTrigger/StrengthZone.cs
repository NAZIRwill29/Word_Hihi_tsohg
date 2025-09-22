using UnityEngine;

public class StrengthZone : StatusZone
{
    [SerializeField] protected StrengthDataFlyweight m_StrengthDataFlyweight;

    public override object CreateEffectData()
    {
        return m_StrengthDataFlyweight.StrengthData;
    }
}
