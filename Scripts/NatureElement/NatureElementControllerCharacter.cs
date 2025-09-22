using System.Collections.Generic;
using UnityEngine;

public class NatureElementControllerCharacter : NatureElementController
{
    protected Character m_Character;
    public List<NatureElementEffectCharacterSO> NatureElementEffectCharacterSOs { get; set; } = new();

    void Awake()
    {
        m_Character = GetComponent<Character>();

        if (m_Character == null)
        {
            Debug.LogError($"ObjectT component is missing on {gameObject.name}!");
        }
    }

    void OnEnable()
    {
        if (m_Character?.ObjectStatsManager == null)
        {
            Debug.LogError($"ObjectStatsManager is missing on {gameObject.name}!");
            return;
        }

        m_Character.ObjectStatsManager.OnTrigger += OnTrigger;
    }

    void OnDisable()
    {
        if (m_Character?.ObjectStatsManager == null) return;
        m_Character.ObjectStatsManager.OnTrigger -= OnTrigger;
    }

    protected override void BenefitEffect()
    {
        foreach (var item in NatureElementEffectCharacterSOs)
        {
            item.BenefitEffect(m_Character);
        }
    }

    protected override void SupportEffect()
    {
        foreach (var item in NatureElementEffectCharacterSOs)
        {
            item.SupportEffect(m_Character);
        }
    }

    protected override void StrengthEffect()
    {
        foreach (var item in NatureElementEffectCharacterSOs)
        {
            item.StrengthEffect(m_Character);
        }
    }

    protected override void WeaknessEffect()
    {
        foreach (var item in NatureElementEffectCharacterSOs)
        {
            item.WeaknessEffect(m_Character);
        }
    }
}
