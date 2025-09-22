using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public interface IObjectStatProvider
{
    ObjectStat GetObjectStatByName(string name);
}

public class ObjectStat : MonoBehaviour
{
    /// <summary>
    /// any -> object(Health) -> (Health) -> graphic
    /// </summary>
    [SerializeField] protected ObjectT m_ObjectT;

    // Main Stats Data
    public StatsData StatsData = new();

    // States
    [HideInInspector] public bool IsInZoneTrigger;

    // Excluded Variable Names
    public string[] InvokeNumVarExcludeName, InvokeBoolVarExcludeName;

    // Optional Object References
    [SerializeField]
    [Optional]
    protected ObjectPoisonable m_ObjectPoisonable;

    [SerializeField]
    [Optional]
    protected ObjectRecoverable m_ObjectRecoverable;

    // Stat Change Lists
    [Tooltip("Optional to fill")]
    public List<StatNumChange> StatsNumChange = new List<StatNumChange>();

    [Tooltip("Optional to fill")]
    public List<StatBoolChange> StatsBoolChange = new List<StatBoolChange>();

    // Serializable Classes for Stat Changes
    [Serializable]
    public class StatNumChange : INameable
    {
        public string NameT;
        public string Name { get => NameT; }
        public UnityEvent<DataNumericalVariable> Change = new UnityEvent<DataNumericalVariable>();
    }

    [Serializable]
    public class StatBoolChange : INameable
    {
        public string NameT;
        public string Name { get => NameT; }
        public UnityEvent<DataBoolVariable> Change = new UnityEvent<DataBoolVariable>();
    }

    public float RecoveryCooldown { get; set; }
    [SerializeField] protected bool IsNoUpdate;
    public bool IsStopRecovery;

    private void OnValidate()
    {
        if (StatsData != null)
        {
            if (StatsData.DataNumVars != null)
            {
                foreach (var item in StatsData.DataNumVars)
                {
                    // Manually trigger validation
                    item.SyncName();
                }
            }
            if (StatsData.DataBoolVars != null)
            {
                foreach (var item in StatsData.DataBoolVars)
                {
                    // Manually trigger validation
                    item.SyncName();
                }
            }
        }
    }

    #region Unity Lifecycle
    protected virtual void Awake()
    {
        InitializeStats();
        if (m_ObjectRecoverable)
            m_ObjectRecoverable.Initialize(this);
    }
    protected virtual void Start()
    {
        //m_ParentId = transform.parent.gameObject.GetInstanceID();

    }

    protected virtual void Update()
    {
        //RCOVERY(1)
        if (IsNoUpdate) return;
        Cooldown();
        if (m_ObjectPoisonable)
        {
            m_ObjectPoisonable.Cooldown(m_ObjectT, this);
            m_ObjectPoisonable.Poison(this);
        }
        if (m_ObjectRecoverable && !IsStopRecovery)
            m_ObjectRecoverable.Recovery(m_ObjectT, this);
    }
    #endregion

    #region Initialization
    private void InitializeStats()
    {
        if (StatsData == null)
        {
            Debug.LogWarning("StatsData is null during initialization.");
            return;
        }

        if (StatsData.DataNumVars != null)
        {
            foreach (var numVar in StatsData.DataNumVars)
            {
                numVar.NumVariableOri = numVar.NumVariable;
                if (numVar.NumVariableMax <= 0)
                    numVar.NumVariableMax = numVar.NumVariable;
                numVar.NumVariableMaxOri = numVar.NumVariableMax;

                bool exists = false;
                foreach (var statChange in StatsNumChange)
                {
                    if (statChange.Name.Equals(numVar.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    StatsNumChange.Add(new StatNumChange { NameT = numVar.Name });
            }
        }

        if (StatsData.DataBoolVars != null)
        {
            foreach (var boolVar in StatsData.DataBoolVars)
            {
                bool exists = false;
                foreach (var statChange in StatsBoolChange)
                {
                    if (statChange.Name.Equals(boolVar.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    StatsBoolChange.Add(new StatBoolChange { NameT = boolVar.Name });
            }
        }
    }
    #endregion

    #region Cooldown Logic
    protected virtual void Cooldown()
    {
        if (StatsData == null) return;
        if (StatsData.DataNumVars == null) return;

        float deltaTime = Time.deltaTime;

        foreach (var item in StatsData.DataNumVars)
        {
            if (!m_ObjectT.IsAlive)
                item.Cooldown = item.TimeCoolDown;
            else
                item.Cooldown -= deltaTime;

            if (item.PoisonDuration <= 0 || !m_ObjectPoisonable)
                item.IsPoison = false;

            if (item.RecoveryNumChangeCooldown >= 0)
            {
                item.RecoveryNumChangeCooldown -= deltaTime;
            }
            else
            {
                if (!item.IsRecoveryNumChangePermanent)
                    item.RecoveryNum = item.RecoveryNumOri;
            }
        }
        foreach (var item in StatsData.DataBoolVars)
        {
            if (item.PoisonDuration <= 0 || !m_ObjectPoisonable)
                item.IsPoison = false; ;
        }
    }
    #endregion

    #region Variable Management
    public virtual void ChangeAllVariable(object data, bool inZone)
    {
        IsInZoneTrigger = inZone;

        if (data is StatsData statsData)
            ObjectStatProcessor.ChangeStatsVariable(this, statsData);
    }

    protected virtual void ChangeAllVariable(object data)
    {
        if (data is StatsData statsData)
        {
            ObjectStatProcessor.ChangeStatsVariable(this, statsData);
        }
    }

    protected virtual void ChangeRecoveryStat(string statName, float num, float duration, bool isPermanent = false)
    {
        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            statName,
            dataNumVar =>
            {
                dataNumVar.RecoveryNum = num;
                dataNumVar.IsRecoveryNumChangePermanent = isPermanent;
                dataNumVar.RecoveryNumChangeCooldown = duration;
            }
        );
    }
    #endregion

    #region Event Handling
    public virtual void PoisonHappened(ListPoisonData listPoisonData)
    {
        if (StatsData == null) return;

        foreach (var variable in StatsData.DataNumVars)
        {
            PoisonData poisonData = null;
            foreach (var data in listPoisonData.PoisonData)
            {
                if (data.Name.Equals(variable.Name, StringComparison.OrdinalIgnoreCase))
                {
                    poisonData = data;
                    break;
                }
            }

            if (poisonData != null && variable.IsPoison && poisonData.IsNumber)
            {
                variable.AddNumVariable = poisonData.AddNum;
                variable.ActivityName = "poison";

                UnityEvent<DataNumericalVariable> unityEvent = ObjectStatProcessor.GetUnityEventInStatNumChange(this, variable.Name);
                unityEvent?.Invoke(variable);
            }
        }

        foreach (var variable in StatsData.DataBoolVars)
        {
            PoisonData poisonData = null;
            foreach (var data in listPoisonData.PoisonData)
            {
                if (data.Name.Equals(variable.Name, StringComparison.OrdinalIgnoreCase))
                {
                    poisonData = data;
                    break;
                }
            }

            if (poisonData != null && variable.IsPoison && !poisonData.IsNumber)
            {
                variable.ActivityName = "poison";

                UnityEvent<DataBoolVariable> unityEvent = ObjectStatProcessor.GetUnityEventInStatBoolChange(this, variable.Name);
                unityEvent?.Invoke(variable);
            }
        }
    }

    public virtual void RecoveryHappened(ListRecoveryData listRecoveryData)
    {
        //RCOVERY(4)
        if (StatsData == null) return;

        ObjectStatProcessor.ProcessVariables(StatsData.DataNumVars, (variable) =>
        {
            if (VariableFinder.TryGetVariableContainNameFromList(listRecoveryData.RecoveryData, variable.Name, out var recoveryData) && recoveryData.IsNumber)
            {
                variable.AddNumVariable = recoveryData.AddNum;

                //float percentageChange = variable.AddNumVariable / variable.NumVariableMax * 100;
                variable.ActivityName = "recovery";

                UnityEvent<DataNumericalVariable> unityEvent = ObjectStatProcessor.GetUnityEventInStatNumChange(this, variable.Name);
                unityEvent?.Invoke(variable);

                //Debug.Log($"Recovery effect on numerical variable {variable.Name}: {variable.AddNumVariable}");
            }
        });

        ObjectStatProcessor.ProcessVariables(StatsData.DataBoolVars, (variable) =>
        {
            if (VariableFinder.TryGetVariableContainNameFromList(listRecoveryData.RecoveryData, variable.Name, out var recoveryData) && recoveryData.IsNumber)
            {
                variable.ActivityName = "recovery";
                UnityEvent<DataBoolVariable> unityEvent = ObjectStatProcessor.GetUnityEventInStatBoolChange(this, variable.Name);
                unityEvent?.Invoke(variable);

                //Debug.Log($"Recovery logic for boolean variable {variable.Name}: IsCan = {variable.IsCan}");
            }
        });
    }
    #endregion

    public virtual void ExitTriggered()
    {
        IsInZoneTrigger = false;
    }
}
