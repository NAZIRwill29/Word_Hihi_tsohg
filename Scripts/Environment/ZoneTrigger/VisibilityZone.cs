using UnityEngine;

public class VisibilityZone : StatusZone
{
    [SerializeField] protected VisibilityDataFlyweight m_VisibilityDataFlyweight;

    public override object CreateEffectData()
    {
        return m_VisibilityDataFlyweight.VisibilityData;
    }
}
