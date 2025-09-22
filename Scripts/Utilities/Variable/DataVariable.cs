//collecttion of data variable - usefull for stats
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public enum BoolNull
{
    IsNull, IsTrue, IsFalse,
}

public class DataVariable
{
    //return the class name as a string
    // public string GetClassName()
    // {
    //     //GetType() - retrieves the runtime Type of the object
    //     //Name - It extracts the name of the class as a string
    //     return GetType().Name;
    // }
}

// [System.Serializable]
// public class TriggerStatsData : DataVariable
// {
//     public List<TriggerDataNumericalVariable> DataNumVars;
//     public List<DataBoolVariable> DataBoolVars;
//     public string OwnerName;
//     public string EffectorName { get; set; }
//     //public string SoundName, FXName;

//     public void SetOri()
//     {
//         foreach (var item in DataNumVars)
//         {
//             item.AddNumVariableOri = item.AddNumVariable;
//             item.NumVariableOri = item.NumVariable;
//         }
//         foreach (var item in DataBoolVars)
//         {
//             item.IsCanOri = item.IsCan;
//         }
//     }

//     public void Revert()
//     {
//         foreach (var item in DataNumVars)
//         {
//             item.AddNumVariable = item.AddNumVariableOri;
//             item.NumVariable = item.NumVariableOri;
//         }
//         foreach (var item in DataBoolVars)
//         {
//             item.IsCan = item.IsCanOri;
//         }
//     }
// }

[System.Serializable]
public class StatsData
{
    public NameDataFlyweight StatDataNameDataFlyweight;
    public List<DataNumericalVariable> DataNumVars;
    public List<DataBoolVariable> DataBoolVars;
    public string OwnerName;
    public string EffectorName { get; set; }
    //public string SoundName, FXName;

    public void SetOri()
    {
        foreach (var item in DataNumVars)
        {
            item.AddNumVariableOri = item.AddNumVariable;
            item.NumVariableOri = item.NumVariable;
        }
        foreach (var item in DataBoolVars)
        {
            item.IsCanOri = item.IsCan;
        }
    }

    public void Revert()
    {
        foreach (var item in DataNumVars)
        {
            item.AddNumVariable = item.AddNumVariableOri;
            item.NumVariable = item.NumVariableOri;
        }
        foreach (var item in DataBoolVars)
        {
            item.IsCan = item.IsCanOri;
        }
    }
}

[System.Serializable]
public class DataBoolVariable : PoisonVariable
{
    public BoolNull IsCan = BoolNull.IsTrue;
    public BoolNull IsCanOri { get; set; }
}

// [System.Serializable]
// public class TriggerDataNumericalVariable : PoisonVariable
// {
//     [Tooltip("if want to decreasing, make it (-)")]
//     public float PoisonAmount;
//     public float PoisonCooldown { get; set; }
//     public float PoisonTimeCoolDown;
//     public bool IsIncrement;
//     public bool IsRecoverMax;
//     [Tooltip("No need for zone or harmnfull")]
//     [SerializeField, Range(0, 999999)] float m_NumVariable;
//     public float NumVariable
//     {
//         get => m_NumVariable;
//         set => m_NumVariable = Mathf.Clamp(value, NumVariableMin, NumVariableMax);
//     }
//     public float NumVariableMin;
//     [Range(1, 999999)] public float NumVariableMax = 100f;
//     [Tooltip("if want to decreasing, make it (-)")]
//     public float AddNumVariable;
//     public float AddNumVariableMax;
//     //float m_NumVariableOri;
//     public float NumVariableOri { get; set; }
//     public float NumVariableMaxOri { get; set; }
//     public float Cooldown { get; set; }
//     public float TimeCoolDown;
//     public float AddNumVariableOri { get; set; }
// }
[System.Serializable]
public class DataNumericalVariable : PoisonVariable
{
    [Tooltip("if want to decreasing, make it (-)")]
    public float PoisonAmount;
    public float PoisonCooldown { get; set; }
    public float PoisonTimeCoolDown;
    public bool IsIncrement;
    public bool IsRecoverMax;
    [Tooltip("No need for zone or harmnfull")]
    [SerializeField, Range(0, 999999)] float m_NumVariable;
    public float NumVariable
    {
        get => m_NumVariable;
        set => m_NumVariable = Mathf.Clamp(value, NumVariableMin, NumVariableMax);
    }
    public float NumVariableMin;
    [Range(1, 999999)] public float NumVariableMax = 100f;
    //public float RecoveryNumChangeTimeCooldown;
    [Tooltip("if want to decreasing, make it (-)")]
    public float AddNumVariable;

    [Tooltip("for increasing or decreasing max stats")]
    public float AddNumVariableMax;
    //float m_NumVariableOri;
    public float NumVariableOri { get; set; }
    public float NumVariableMaxOri { get; set; }
    public float Cooldown { get; set; }
    public float TimeCoolDown;
    public float AddNumVariableOri { get; set; }
    public float RecoveryNumOri = 1f;
    public float RecoveryNum = 1f;
    public float RecoveryNumChangeCooldown { get; set; }
    public bool IsRecoveryNumChangePermanent { get; set; }
}

[System.Serializable]
public class PoisonVariable : NameActVariable
{
    public bool IsPoison;
    public float PoisonDuration;
    public bool IsPoisonImmune;
}
[System.Serializable]
public class NameActVariable : NameVariable
{
    public string ActivityName;
    public BoolNull IsCanRecovery = BoolNull.IsTrue;
    public bool IsCanRecoveryInNormal;
    public bool IsCanRecoveryInBattle;
    public float DelayRecoveryCooldown { get; set; }
}
[System.Serializable]
public class NameVariable : ISettableName
{
    [ReadOnly][SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : m_Name;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    public void SyncName()
    {
        m_Name = NameDataFlyweight != null ? NameDataFlyweight.Name : string.Empty;
    }
    public void SetName(string name)
    {
        Name = name;
    }
}
