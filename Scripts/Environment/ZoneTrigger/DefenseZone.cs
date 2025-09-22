using UnityEngine;

public class DefenseZone : StatusZone
{
    [SerializeField] protected DefenseDataFlyweight m_DefenseDataFlyweight;

    public override object CreateEffectData()
    {
        return m_DefenseDataFlyweight.DefenseData;
    }

    // public override ITriggerable CreateEffect()
    // {
    //     return new DefenseEffect();
    // }
}
