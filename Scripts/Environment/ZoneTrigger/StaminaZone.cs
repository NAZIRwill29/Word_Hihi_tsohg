using UnityEngine;

public class StaminaZone : StatusZone
{
    [SerializeField] protected StaminaDataFlyweight m_StaminaDataFlyweight;

    public override object CreateEffectData()
    {
        return m_StaminaDataFlyweight.StaminaData;
    }
}