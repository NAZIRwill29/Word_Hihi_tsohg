using UnityEngine;

public class EvadeZone : StatusZone
{
    [SerializeField] protected EvadeDataFlyweight m_EvadeDataFlyweight;

    public override object CreateEffectData()
    {
        return m_EvadeDataFlyweight.EvadeData;
    }
}
