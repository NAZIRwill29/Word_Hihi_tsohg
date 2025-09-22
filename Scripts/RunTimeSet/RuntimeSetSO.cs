using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    //[HideInInspector]
    public List<T> Items = new List<T>();
    public void Add(T thing)
    {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }
    public void Remove(T thing)
    {
        if (Items.Contains(thing))
            Items.Remove(thing);
    }
    public void RemoveAll(Predicate<T> match)
    {
        Items.RemoveAll(match);
    }
    public bool TryGetValue(Predicate<T> match, out T value)
    {
        value = Items.Find(match);
        return !EqualityComparer<T>.Default.Equals(value, default(T));
    }

    public void Clear()
    {
        Items.Clear();
    }
}