using UnityEngine;
using System.Collections.Generic;
using System;

public interface INameable
{
    string Name { get; }
}

public interface ISettableName : INameable
{
    void SetName(string name);
}
public static class VariableFinder
{
    /// <summary>
    /// Retrieve any variable that contains the name from a given list.
    /// </summary>
    public static T GetVariableContainNameFromList<T>(List<T> list, string name) where T : class, INameable
    {
        if (list == null || list.Count == 0 || string.IsNullOrEmpty(name))
            return null;

        for (int i = 0; i < list.Count; i++)
        {
            if (string.Equals(list[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return list[i];
            }
        }
        //Debug.Log("Variable " + name + " not found in " + list);
        return null;
    }

    /// <summary>
    /// Retrieve any variable from a given list.
    /// </summary>
    public static bool TryGetVariableFromList<T>(List<T> list, string variableName, out T result) where T : class, INameable
    {
        result = null;

        if (list == null || list.Count == 0 || string.IsNullOrEmpty(variableName))
            return false;

        for (int i = 0; i < list.Count; i++)
        {
            if (string.Equals(list[i].Name, variableName, StringComparison.OrdinalIgnoreCase))
            {
                result = list[i];
                return true;
            }
        }
        //Debug.Log("Variable " + variableName + " not found in " + list);
        return false;
    }

    /// <summary>
    /// Retrieve any variable containing the name from a given list.
    /// </summary>
    public static bool TryGetVariableContainNameFromList<T>(List<T> list, string name, out T result) where T : class, INameable
    {
        //RCOVERY(5)
        result = null;

        if (list == null || list.Count == 0 || string.IsNullOrEmpty(name))
        {
            Debug.LogError("list empty");
            return false;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (string.Equals(list[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                result = list[i];
                return true;
            }
        }
        //Debug.Log("Variable " + name + " not found in " + list);
        return false;
    }

    /// <summary>
    /// Retrieve any variable containing the name from a given array.
    /// </summary>
    public static bool TryGetVariableContainNameFromArray<T>(T[] array, string name, out T result) where T : class, INameable
    {
        result = null;

        if (array == null || array.Length == 0 || string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < array.Length; i++)
        {
            if (string.Equals(array[i].Name, name, StringComparison.OrdinalIgnoreCase))
            {
                result = array[i];
                return true;
            }
        }
        //Debug.Log("Variable " + name + " not found in " + array);
        return false;
    }

    /// <summary>
    /// Retrieve any variable from a given array.
    /// </summary>
    public static bool TryGetVariableFromArray<T>(T[] array, T variable, out T result) where T : class
    {
        result = null;

        if (array == null || array.Length == 0 || variable == null)
            return false;

        for (int i = 0; i < array.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(array[i], variable))
            {
                result = array[i];
                return true;
            }
        }
        //Debug.Log("Variable " + variable + " not found in " + array);
        return false;
    }
}
