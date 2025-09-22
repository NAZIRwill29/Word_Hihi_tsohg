using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum WeaponFrom
// {
//     player, enemy
// }

public class Weapon : ItemObjectT
{
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    // [SerializeField ] WeaponFrom m_WeaponFrom;
    //TODO() - swing, or whatsoever, make it harmfull when want swing

    [SerializeField] private WeaponRuntimeSetSO RuntimeSet;

    protected void OnEnable()
    {
        if (RuntimeSet) RuntimeSet.Add(this);
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
    }
    protected void OnDisable()
    {
        if (RuntimeSet) RuntimeSet.Remove(this);
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
    }
}
