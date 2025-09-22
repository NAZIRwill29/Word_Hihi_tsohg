using UnityEngine;

public class MeleableZone : StatusZone
{
    [SerializeField] protected MeleeDataFlyweight m_MeleeDataFlyweight;

    public override object CreateEffectData()
    {
        return m_MeleeDataFlyweight.MeleeData;
    }
}
