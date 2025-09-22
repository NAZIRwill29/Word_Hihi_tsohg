using UnityEngine;
using UnityEngine.Events;

public class HealthStateManager : MonoBehaviour
{
    //private Character m_Character;
    // private HealthSystem m_HealthSystem;
    private ObjectHealth m_ObjectHealth;
    public float HealthSeparatorNum = 20f;
    private bool m_IsHighHealth = true;
    [SerializeField] private NameDataFlyweight AliveStatBoolNameData;
    public UnityEvent OnLowHealth, OnHighHealth;

    public void Activate(Character character)
    {
        m_IsHighHealth = true;

        if (m_ObjectHealth)
            Deactivate();

        m_ObjectHealth = character.ObjectHealth;
        //m_Character = character;
        //m_HealthSystem = character.HealthSystem;

        m_ObjectHealth.HealthChanged.AddListener(HealthChanged);
        // m_HealthSystem.Died.AddListener(Died);
        //ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectHealth, AliveStatBoolNameData.Name).AddListener(NotifyAliveChanged);
    }

    public void Deactivate()
    {
        m_IsHighHealth = true;

        if (!m_ObjectHealth) return;

        m_ObjectHealth.HealthChanged.RemoveListener(HealthChanged);
        //m_HealthSystem.Died.RemoveListener(Died);
        //ObjectStatProcessor.GetUnityEventInStatBoolChange(m_ObjectHealth, AliveStatBoolNameData.Name).RemoveListener(NotifyAliveChanged);
    }

    private void HealthChanged(StatsMicrobarData statsMicrobarData)
    {
        DataNumericalVariable data = VariableFinder.GetVariableContainNameFromList(statsMicrobarData.DataNumVars, "Health");
        if (data != null)
        {
            if (data.NumVariable < HealthSeparatorNum)
            {
                if (!m_IsHighHealth) return;
                OnLowHealth?.Invoke();
                m_IsHighHealth = false;
            }
            else
            {
                if (m_IsHighHealth) return;
                OnHighHealth?.Invoke();
                m_IsHighHealth = true;
            }
        }
    }

    // private void NotifyAliveChanged(DataBoolVariable dataBoolVariable)
    // {
    //     if (m_Character.IsAlive == VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
    //         return;
    //     if (VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
    //         OnDied.Invoke();
    // }

    // private void Died()
    // {
    //     OnDied.Invoke();
    // }
}
