using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListWrapper<T>
{
    [SerializeField] private List<T> items = new();

    public void Add(T item)
    {
        items.Add(item);
    }

    public void Remove(T item)
    {
        items.Remove(item);
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool ContainsKey(T key)
    {
        return items.Contains(key);
    }

    public T Find(Predicate<T> match)
    {
        return items.Find(match);
    }

    public T FindByName(string name)
    {
        return items.Find(item => item.ToString().Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public bool TryGetValue(T key, out T value)
    {
        if (items.Contains(key))
        {
            value = key;
            return true;
        }
        value = default;
        return false;
    }

    // public bool TryGetValueByName(string name, out T value)
    // {
    //     value = items.Find(item => item.ToString().Equals(name, StringComparison.OrdinalIgnoreCase));
    //     return value != null;
    // }

    public bool TryGetValue(Predicate<T> match, out T value)
    {
        value = items.Find(match);
        return !EqualityComparer<T>.Default.Equals(value, default(T));
    }

    public bool TryGetIndex(Predicate<T> match, out int index)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (match(items[i]))
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public int GetIndex(Predicate<T> match)
    {
        return TryGetIndex(match, out int index) ? index : -1;
    }

    public List<T> Items => items; // Expose underlying list if needed
}
