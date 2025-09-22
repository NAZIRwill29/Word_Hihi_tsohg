using System;
using System.Linq;
using UnityEngine;

//TODO()
public class NatureElementControllerItemObjectT : NatureElementController
{
    protected ItemObjectT m_ItemObjectT;
    public NatureElementEffectItemObjectTSO[] NatureElementEffectItemObjectTSOs;

    void Awake()
    {
        m_ItemObjectT = GetComponent<ItemObjectT>();
    }

    void OnEnable()
    {
        m_ItemObjectT.OnTriggerEffect += OnTrigger;
    }

    void OnDisable()
    {
        m_ItemObjectT.OnTriggerEffect -= OnTrigger;
    }

    protected override void BenefitEffect()
    {
        foreach (var item in NatureElementEffectItemObjectTSOs)
        {
            item.BenefitEffect(m_ItemObjectT);
        }
    }

    protected override void SupportEffect()
    {
        foreach (var item in NatureElementEffectItemObjectTSOs)
        {
            item.SupportEffect(m_ItemObjectT);
        }
    }

    protected override void StrengthEffect()
    {
        foreach (var item in NatureElementEffectItemObjectTSOs)
        {
            item.StrengthEffect(m_ItemObjectT);
        }
    }

    protected override void WeaknessEffect()
    {
        foreach (var item in NatureElementEffectItemObjectTSOs)
        {
            item.WeaknessEffect(m_ItemObjectT);
        }
    }
}
