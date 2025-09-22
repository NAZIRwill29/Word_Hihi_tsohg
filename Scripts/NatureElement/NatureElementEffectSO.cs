using UnityEngine;

public class NatureElementEffectSO : ScriptableObject
{
    [SerializeField] protected NameDataFlyweight m_NameDataFlyweight;
    [SerializeField] protected float m_BenefitNum, m_SupportNum, m_StrengthNum, m_WeaknessNum;
    [SerializeField] protected NameDataFlyweight m_MonoBehaviourNameDataFlyweight;
}
