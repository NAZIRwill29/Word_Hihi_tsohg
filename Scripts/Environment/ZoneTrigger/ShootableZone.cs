using UnityEngine;

public class ShootableZone : StatusZone
{
    [SerializeField] protected ShootDataFlyweight m_ShootDataFlyweight;

    public override object CreateEffectData()
    {
        return m_ShootDataFlyweight.ShootData;
    }
}