using System;
using UnityEngine;
using UnityEngine.Events;

public class StabilitySystem : MircobarSystem
{
    [SerializeField] private ObjectStability m_ObjectStability;

    void OnEnable()
    {
        if (!m_ObjectStability)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectStability, StatNumberNameData.Name).AddListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectStability, StatBoolNameData.Name).AddListener(NotifyAliveChanged);
        m_ObjectStability.StabilityChanged.AddListener(NotifyStatChanged);
    }

    void OnDisable()
    {
        if (!m_ObjectStability)
            return;
        ObjectStatProcessor.GetUnityEventInStatNumChange(m_ObjectStability, StatNumberNameData.Name).RemoveListener(NotifyStatChanged);
        ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectStability, StatBoolNameData.Name).RemoveListener(NotifyAliveChanged);
        m_ObjectStability.StabilityChanged.AddListener(NotifyStatChanged);
    }

    public void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        if (m_ResetOnStart)
        {
            ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
                m_ObjectStability,
                m_ObjectStability.StatsData.DataNumVars,
                StatNumberNameData.Name,
                dataNumVar => dataNumVar.NumVariable = m_IsNumStartMaxVariable ? dataNumVar.NumVariableMax : dataNumVar.NumVariableMin
            );
        }
        float currentHealth = VariableFinder.GetVariableContainNameFromList(m_ObjectStability.StatsData.DataNumVars, StatNumberNameData.Name).NumVariable;
        StatInit.Invoke(currentHealth);
    }
}
