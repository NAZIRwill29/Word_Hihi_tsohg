using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// public class HealthChangeData
// {
//     public float healthPercentageChange;
//     public MicrobarAnimType microbarAnimType = MicrobarAnimType.simple;
//     //public string soundName;
// }

//get time from statTrigger to Microbar = 0.035, 0.01, 0.005

/// <summary>
/// Handles object health tracking and updates the health bar.
/// </summary>
public class HealthSystem : MircobarSystem
{
    [SerializeField] private ObjectHealth m_ObjectHealth;
    void OnEnable()
    {
        if (!m_ObjectHealth)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectHealth, StatNumberNameData.Name).AddListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectHealth, StatBoolNameData.Name).AddListener(NotifyAliveChanged);
        m_ObjectHealth.HealthChanged.AddListener(NotifyStatChanged);
    }

    void OnDisable()
    {
        if (!m_ObjectHealth)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectHealth, StatNumberNameData.Name).RemoveListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectHealth, StatBoolNameData.Name).RemoveListener(NotifyAliveChanged);
        m_ObjectHealth.HealthChanged.AddListener(NotifyStatChanged);
    }

    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        if (m_ResetOnStart)
        {
            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectHealth,
                m_ObjectHealth.StatsData.DataNumVars,
                StatNumberNameData.Name,
                dataNumVar => dataNumVar.NumVariable = m_IsNumStartMaxVariable ? dataNumVar.NumVariableMax : dataNumVar.NumVariableMin
            );
        }
        float currentHealth = VariableFinder.GetVariableContainNameFromList(m_ObjectHealth.StatsData.DataNumVars, StatNumberNameData.Name).NumVariable;
        StatInit.Invoke(currentHealth);
    }
}
