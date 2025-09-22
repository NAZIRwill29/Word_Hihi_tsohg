using UnityEngine;

public class LevelZone : StatusZone
{
    [SerializeField] protected LevelDataFlyweight m_LevelDataFlyweight;

    public override object CreateEffectData()
    {
        return m_LevelDataFlyweight.LevelData;
    }
}