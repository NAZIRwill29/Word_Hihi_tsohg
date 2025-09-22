using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class ObjectStatProcessor
{
    public static void ChangeStatsVariable(ObjectStat objectStat, StatsData data)
    {
        if (data == null || objectStat.StatsData == null) return;

        objectStat.StatsData.EffectorName = data.OwnerName;

        VariableChanger.ChangeNameVariables(
            data.DataBoolVars,
            objectStat.StatsData.DataBoolVars,
            CreateDataBoolVariable,
            (source, target) => ProcessBoolVariable(objectStat, source, target)
        );

        VariableChanger.ChangeNameVariables(
            data.DataNumVars,
            objectStat.StatsData.DataNumVars,
            CreateDataNumericalVariable,
            (source, target) => ProcessNumVariable(objectStat, source, target)
        );
    }


    private static DataBoolVariable CreateDataBoolVariable(string name)
    {
        return new DataBoolVariable { Name = name };
    }

    private static DataNumericalVariable CreateDataNumericalVariable(string name)
    {
        return new DataNumericalVariable { Name = name };
    }

    private static void ProcessBoolVariable(ObjectStat objectStat, DataBoolVariable source, DataBoolVariable target)
    {
        if (!FindInvokeExcludeName(objectStat, target.Name))
        {
            Debug.Log("ChangeBoolVariable " + target.Name);
            target.IsCan = source.IsCan != BoolNull.IsNull ? source.IsCan : target.IsCan;

            if (source.IsPoison && !target.IsPoisonImmune)
            {
                target.PoisonDuration = source.PoisonDuration > 0
                    ? source.PoisonDuration >= target.PoisonDuration
                        ? source.PoisonDuration
                        : source.PoisonDuration + target.PoisonDuration
                    : target.PoisonDuration;

                target.IsPoison = true;
            }

            UnityEvent<DataBoolVariable> unityEvent = GetUnityEventInStatBoolChange(objectStat, target.Name);
            unityEvent?.Invoke(target);
        }
    }

    private static void ProcessNumVariable(ObjectStat objectStat, DataNumericalVariable source, DataNumericalVariable target)
    {
        if (!FindInvokeExcludeName(objectStat, target.Name))
        {
            if (target.Cooldown <= 0)
            {
                target.AddNumVariable = source.AddNumVariable;
                target.NumVariable = source.IsIncrement
                    ? target.NumVariable + target.AddNumVariable
                    : target.NumVariableOri + target.AddNumVariable;

                target.AddNumVariableMax = source.AddNumVariableMax;
                target.NumVariableMax = source.IsIncrement
                    ? target.NumVariableMax + target.AddNumVariableMax
                    : target.NumVariableMaxOri + target.AddNumVariableMax;

                target.NumVariableOri = source.IsIncrement ? target.NumVariable : target.NumVariableOri;
                target.NumVariableMaxOri = source.IsIncrement ? target.NumVariableMax : target.NumVariableMaxOri;

                target.TimeCoolDown = source.TimeCoolDown > 0 ? source.TimeCoolDown : target.TimeCoolDown;
                target.Cooldown = source.TimeCoolDown;

                UnityEvent<DataNumericalVariable> unityEvent = GetUnityEventInStatNumChange(objectStat, target.Name);
                unityEvent?.Invoke(target);
            }
        }
    }
    #region Helpers

    /// <summary>
    /// // Update a variable in list
    /// </summary>
    /// <param name="list"></param>
    /// <param name="variableName"></param>
    /// <param name="changeAction"></param>
    // UpdateVariableInList(numericalList, "Health", variable =>
    // {
    //     if (variable is DataNumericalVariable dataNumVar)
    //     {
    //         dataNumVar.NumVariable = 100f;
    //     }
    // });
    public static void UpdateVariableInListWithInvokeEvent<T>(
        ObjectStat objectStat, List<T> list, string variableName, Action<T> changeAction) where T : class, INameable, new()
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError("List is null or empty. Cannot update variable.");
            return;
        }

        if (VariableFinder.TryGetVariableFromList(list, variableName, out T targetVar))
        {
            changeAction(targetVar);
            InvokeUnityEvent(objectStat, variableName, targetVar);
        }
        else
        {
            var newVariable = new T();

            // Workaround for read-only Name property:
            if (newVariable is ISettableName settable)
            {
                settable.SetName(variableName);
            }
            else
            {
                Debug.LogError($"Type {typeof(T).Name} does not allow setting Name. Implement ISettableName.");
                return;
            }

            list.Add(newVariable);
            changeAction(newVariable);
            InvokeUnityEvent(objectStat, variableName, newVariable);
        }
    }

    // New static method to avoid capturing variables in lambdas
    private static void InvokeUnityEvent(ObjectStat objectStat, string variableName, object variable)
    {
        if (variable is DataNumericalVariable dataNumericalVariable)
        {
            InvokeUnityEventInStatNumChange(objectStat, variableName, dataNumericalVariable);
        }
        else if (variable is DataBoolVariable dataBoolVariable)
        {
            InvokeUnityEventInStatBoolChange(objectStat, variableName, dataBoolVariable);
        }
    }

    public static void InvokeUnityEventInStatNumChange(ObjectStat objectStat, string name, DataNumericalVariable dataNumericalVariable)
    {
        if (FindInvokeExcludeName(objectStat, name))
            return;
        // Invoke the corresponding UnityEvent if it exists
        var unityEvent = GetUnityEventInStatNumChange(objectStat, name);
        if (unityEvent != null)
        {
            unityEvent.Invoke(dataNumericalVariable);
        }
        else
        {
            Debug.LogWarning($"No UnityEvent found for variable '{name}'.");
        }
    }

    public static void InvokeUnityEventInStatBoolChange(ObjectStat objectStat, string name, DataBoolVariable dataBoolVariable)
    {
        if (FindInvokeExcludeName(objectStat, name))
            return;
        // Invoke the corresponding UnityEvent if it exists
        var unityEvent = GetUnityEventInStatBoolChange(objectStat, name);
        if (unityEvent != null)
        {
            unityEvent.Invoke(dataBoolVariable);
        }
        else
        {
            Debug.LogWarning($"No UnityEvent found for variable '{name}'.");
        }
    }

    /*
    The ProcessVariables method is a generic helper method designed to streamline the iteration and 
    processing of variables (both numerical and boolean) in the ObjectStat class. It abstracts the common 
    logic of iterating through arrays of variables and performing actions on each element, making the code 
    more concise and reusable.
    Generic Type Parameter (T):
    -The method is generic and works with any type (T). In this context, it's typically used with 
     DataNumericalVariable and DataBoolVariable.
    Parameters:
    -T[] variables: An array of variables to be processed.
    -Action<T> action: A delegate (or callback function) representing the action to perform on each 
     variable in the array.
    */
    public static void ProcessVariables<T>(List<T> variables, Action<T> action)
    {
        if (variables == null) return;

        foreach (var variable in variables)
        {
            action?.Invoke(variable);
        }
    }

    public static bool FindInvokeExcludeName(ObjectStat objectStat, string name)
    {
        if (objectStat.InvokeNumVarExcludeName != null)
        {
            if (objectStat.InvokeNumVarExcludeName.Length <= 0)
                return false;
            for (int i = 0; i < objectStat.InvokeNumVarExcludeName.Length; i++)
            {
                if (name.Equals(objectStat.InvokeNumVarExcludeName[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        if (objectStat.InvokeBoolVarExcludeName != null)
        {
            if (objectStat.InvokeBoolVarExcludeName.Length <= 0)
                return false;
            for (int i = 0; i < objectStat.InvokeBoolVarExcludeName.Length; i++)
            {
                if (name.Equals(objectStat.InvokeBoolVarExcludeName[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }
        }

        return false;
    }

    public static UnityEvent<DataNumericalVariable> GetUnityEventInStatNumChange(ObjectStat objectStat, string name)
    {
        if (VariableFinder.TryGetVariableContainNameFromList(objectStat.StatsNumChange, name, out ObjectStat.StatNumChange statNumChange))
            return statNumChange.Change;
        else
        {
            Debug.LogWarning("failed GetUnityEventInStatNumChange");
            return null;
        }
    }

    public static UnityEvent<DataBoolVariable> GetUnityEventInStatBoolChange(ObjectStat objectStat, string name)
    {
        if (VariableFinder.TryGetVariableContainNameFromList(objectStat.StatsBoolChange, name, out ObjectStat.StatBoolChange statBoolChange))
            return statBoolChange.Change;
        else
        {
            Debug.LogWarning("failed GetUnityEventInStatBoolChange");
            return null;
        }
    }
    #endregion
}
