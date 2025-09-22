using System;
using Unity.VisualScripting;
using UnityEngine;

public class StatDrainOverTime : MonoBehaviour
{
    [SerializeField] protected StatsMicrobarDataFlyWeight m_StatsMicrobarDataFlyWeight;
    [ReadOnly] public float DrainTime;
    private float m_DrainCooldown;
    [SerializeField] private float m_DrainTimeOri = 5;
    private bool m_IsActive;

    void Update()
    {
        if (!m_IsActive) return;

        m_DrainCooldown -= Time.deltaTime;
        if (m_DrainCooldown >= 0) return;

        m_DrainCooldown = DrainTime;
        DrainStat();
    }

    public virtual void Activate(bool isTrue)
    {
        m_IsActive = isTrue;
        ResetDrainCooldown();
    }

    protected virtual void DrainStat()
    {
        // if (ObjectStat != null && ObjectStat is IDrain drain)
        //     drain.DrainStat(m_StatsMicrobarData);
    }

    // public virtual void ChangeDrainVariable(StatsMicrobarData data)
    // {

    // }

    public void ResetDrainCooldown()
    {
        DrainTime = m_DrainTimeOri;
    }
}
