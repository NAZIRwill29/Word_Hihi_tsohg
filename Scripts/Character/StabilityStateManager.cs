using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StabilityStateManager : MonoBehaviour
{
    //private Character m_Character;
    private ObjectStability m_ObjectStability;
    public NameDataFlyweight HighStateNameData;
    public NameDataFlyweight LowStateNameData;
    public float StabilitySeparatorNum = 20f;
    private bool m_IsHighStability = true;
    private bool m_IsZeroStability = false;
    [SerializeField] private NameDataFlyweight AliveStatBoolNameData;
    public UnityEvent<string> OnLowStability, OnHighStability, OnZeroStability;

    public void Activate(Character character)
    {
        m_IsHighStability = true;
        m_IsZeroStability = false;

        if (m_ObjectStability)
            Deactivate();

        //m_Character = character;
        m_ObjectStability = character.ObjectStability;
        m_ObjectStability.StabilityChanged.AddListener(StabilityChanged);
        //ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectStability, AliveStatBoolNameData.Name).AddListener(NotifyAliveChanged);
    }

    public void Deactivate()
    {
        m_IsHighStability = true;
        m_IsZeroStability = false;

        if (!m_ObjectStability) return;
        m_ObjectStability.StabilityChanged.RemoveListener(StabilityChanged);
        //ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectStability, AliveStatBoolNameData.Name).RemoveListener(NotifyAliveChanged);
    }

    private void StabilityChanged(StatsMicrobarData statsMicrobarData)
    {
        DataNumericalVariable data = VariableFinder.GetVariableContainNameFromList(statsMicrobarData.DataNumVars, "Stability");
        if (data != null)
        {

            if (data.NumVariable <= 0)
            {
                if (m_IsZeroStability) return;
                OnZeroStability?.Invoke(LowStateNameData.Name);
                m_IsZeroStability = true;
                m_IsHighStability = false;
            }
            else if (data.NumVariable < StabilitySeparatorNum)
            {
                if (!m_IsHighStability) return;
                OnLowStability?.Invoke(LowStateNameData.Name);
                m_IsHighStability = false;
                m_IsZeroStability = false;
            }
            else
            {
                if (m_IsHighStability) return;
                OnHighStability?.Invoke(HighStateNameData.Name);
                m_IsHighStability = true;
                m_IsZeroStability = false;
            }
        }
    }

    // private void NotifyAliveChanged(DataBoolVariable dataBoolVariable)
    // {
    //     if (m_Character.IsAlive == VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
    //         return;
    //     if (VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
    //         Died.Invoke();
    // }
}
