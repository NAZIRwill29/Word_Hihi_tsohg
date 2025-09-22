using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VariableChanger
{
    public static bool IsBoolNull(BoolNull boolNull)
    {
        switch (boolNull)
        {
            case BoolNull.IsTrue:
                return true;
            case BoolNull.IsFalse:
                return false;
            default:
                return false;
        }
    }

    /// <summary>
    /// update a property of any object within an array, provided the objects have a Name or similar identifying property
    /// </summary>
    //     var numericalVariables = new DataNumericalVariable[]
    //     {
    //     new DataNumericalVariable { Name = "Health", NumVariable = 50f, Cooldown = 2f },
    //     new DataNumericalVariable { Name = "Poison", PoisonAmount = 10f }
    //     };
    //     var boolVariables = new DataBoolVariable[]
    //     {
    //     new DataBoolVariable { Name = "IsAlive", IsCan = true },
    //     new DataBoolVariable { Name = "CanAttack", IsCan = false }
    //     };
    //     // Update a numerical variable
    //     UpdateVariableInArray(numericalVariables.Cast<object>().ToList(), "Health", "NumVariable", 75f); // Updates Health.NumVariable to 75f
    //     // Update a bool variable
    //     UpdateVariableInArray(boolVariables.Cast<object>().ToList(), "IsAlive", "IsCan", false); // Updates IsAlive.IsCan to false

    public static void UpdateVariableInList<T>(List<T> list, string variableName, Action<T> updateAction) where T : class, INameable, new()
    {
        // Validate input
        if (list == null || list.Count == 0)
        {
            Debug.LogError("List is null or empty. Cannot update.");
            return;
        }

        if (string.IsNullOrEmpty(variableName))
        {
            Debug.LogWarning("Variable name is null or empty. Cannot update.");
            return;
        }

        if (updateAction == null)
        {
            Debug.LogWarning("Update action is null. Cannot update.");
            return;
        }

        // Find the object by name
        T targetObject = list.Find(item => string.Equals(item.Name, variableName, StringComparison.OrdinalIgnoreCase));

        // If not found, create a new one and add it
        if (targetObject == null)
        {
            Debug.LogWarning($"Object with name '{variableName}' not found in the list. Creating new object.");
            if (targetObject is ISettableName settable)
            {
                settable.SetName(variableName);
            }
            else
            {
                Debug.LogError($"Type {typeof(T).Name} does not allow setting Name. Implement ISettableName.");
                return;
            }
            list.Add(targetObject);
        }

        // Apply the update action
        updateAction(targetObject);

        Debug.Log($"Successfully updated '{variableName}'.");
    }

    /// <summary>
    /// Updates matching variables in target collection using a specified action.
    /// </summary>
    /// <typeparam name="T">Type of variable, must inherit from NameVariable.</typeparam>
    /// <param name="sourceVars">Source array of variables to match.</param>
    /// <param name="targetVars">Target array of variables to update.</param>
    /// <param name="changeAction">Action to apply on matching variables.</param>
    //ChangeVar(data.DataBoolVars, StatsData.DataBoolVars, (source, target) =>{target.IsCan = source.IsCan;});
    public static void ChangeNameVariables<T>(
        List<T> sourceVars,
        List<T> targetVars,
        Func<string, T> createNewAction,
        Action<T, T> changeAction
    ) where T : NameVariable
    {
        if (sourceVars == null || targetVars == null || changeAction == null || createNewAction == null) return;

        foreach (var sourceVar in sourceVars)
        {
            if (VariableFinder.TryGetVariableContainNameFromList(targetVars, sourceVar.Name, out T targetVar))
            {
                changeAction(sourceVar, targetVar);
            }
            else
            {
                // Create and add a new variable if not found
                T newVar = createNewAction(sourceVar.Name);
                if (newVar != null)
                {
                    targetVars.Add(newVar);
                    changeAction(sourceVar, newVar);
                }
            }
        }
    }

    /// <summary>
    /// Updates a specific variable by name in a collection using a specified action.
    /// </summary>
    /// <typeparam name="T">Type of variable, must inherit from NameVariable.</typeparam>
    /// <param name="variables">Array of variables to search.</param>
    /// <param name="name">Name of the variable to update.</param>
    /// <param name="changeAction">Action to apply on the matched variable.</param>    /// 
    //ChangeVariable(StatsData.DataBoolVars, "Health", targetVar =>{ targetVar.IsCan = false; });
    public static void ChangeNameVariable<T>(List<T> variables, string name, Func<string, T> createNewAction, Action<T> changeAction) where T : NameVariable
    {
        if (variables == null || string.IsNullOrEmpty(name) || changeAction == null || createNewAction == null) return;

        if (VariableFinder.TryGetVariableContainNameFromList(variables, name, out T targetVar))
        {
            changeAction(targetVar);
        }
        else
        {
            // Create a new variable if not found
            T newVar = createNewAction(name);
            if (newVar != null)
            {
                variables.Add(newVar);
                changeAction(newVar);
            }
        }
    }

    /// <summary>
    /// convert any generic list to a List<object>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceList"></param>
    /// <returns></returns>
    public static List<object> ConvertListToObjectList<T>(List<T> sourceList) where T : class
    {
        if (sourceList == null || sourceList.Count == 0)
            return new List<object>();

        List<object> objectList = new List<object>(sourceList.Count);
        foreach (T item in sourceList)
        {
            objectList.Add(item);
        }
        return objectList;
    }
}

