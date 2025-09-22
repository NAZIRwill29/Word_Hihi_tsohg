using UnityEngine;

public class MagicZone : StatusZone
{
    [SerializeField] protected MagicDataFlyweight m_MagicDataFlyweight;

    public override object CreateEffectData()
    {
        return m_MagicDataFlyweight.MagicData;
    }
}