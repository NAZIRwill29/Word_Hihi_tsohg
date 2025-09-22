using System;
using UnityEngine;
using UnityEngine.Events;

public class MircobarSystem : MonoBehaviour
{
    protected ObjectT m_ObjectT;
    [SerializeField] protected bool m_ResetOnStart;
    [SerializeField] protected bool m_IsNumStartMaxVariable = true;
    [SerializeField] protected MicrobarAnimType m_RecoveryBarAnimType, m_PoisonBarAnimType;
    [SerializeField] protected NameDataFlyweight StatNumberNameData, StatBoolNameData;

    [Tooltip("Notifies listeners that this object has died")]
    public UnityEvent Died;

    [Tooltip("Notifies listeners of updated health percentage")]
    public UnityEvent<float, MicrobarAnimType> StatChanged;

    public UnityEvent<float> StatInit;

    void Awake()
    {
        m_ObjectT = GetComponentInParent<ObjectT>();
    }

    public virtual void Initialize()
    {

    }

    //call when Health changed
    protected void NotifyStatChanged(StatsData statsData)
    {
        if (statsData.DataNumVars == null)
            return;
        //Debug.Log("NotifyHealthChanged 1");

        DataNumericalVariable variable = VariableFinder.GetVariableContainNameFromList(statsData.DataNumVars, StatNumberNameData.Name);
        if (statsData is StatsMicrobarData statsMicrobarData)
            NotifyStatChanged(variable, statsMicrobarData.MicrobarAnimType);
    }

    protected void NotifyStatChanged(DataNumericalVariable variable)
    {
        NotifyStatChanged(variable, MicrobarAnimType.simple);
    }
    protected void NotifyStatChanged(DataNumericalVariable variable, MicrobarAnimType microbarAnimType)
    {
        if (variable == null || variable.AddNumVariable == 0)
            return;

        //Debug.Log("health() - NotifyHealthChanged " + GetTime.GetCurrentTime("full-ms"));
        //Debug.Log(StatNumberNameData.Name + " () - NotifyStatChanged " + variable.NumVariable);
        float percentageChange = variable.AddNumVariable / variable.NumVariableMax * 100;
        MicrobarAnimType animType = DecideBarAnimType(variable.ActivityName, microbarAnimType);
        //string soundName = string.IsNullOrEmpty(variable.ActivityName) ? (!m_ObjectT.IsAlive ? "revive" : "heal") : variable.ActivityName;

        StatChanged.Invoke(percentageChange, animType);
    }

    protected void NotifyAliveChanged(DataBoolVariable dataBoolVariable)
    {
        if (m_ObjectT.IsAlive == VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
            return;
        if (!VariableChanger.IsBoolNull(dataBoolVariable.IsCan))
            Died.Invoke();
    }

    protected MicrobarAnimType DecideBarAnimType(string name, MicrobarAnimType microbarAnimType)
    {
        if (string.IsNullOrEmpty(name))
            return microbarAnimType;
        return name.Equals("recovery", StringComparison.OrdinalIgnoreCase) ? m_RecoveryBarAnimType :
               name.Equals("poison", StringComparison.OrdinalIgnoreCase) ? m_PoisonBarAnimType :
               microbarAnimType;
    }
}
