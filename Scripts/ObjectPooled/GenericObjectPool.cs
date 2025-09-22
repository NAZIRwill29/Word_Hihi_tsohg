using System;
using System.Collections.Generic;

public static class GenericObjectPool<T> where T : class, new()
{
    private static readonly Queue<T> _pool = new();

    public static T Get()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : new T();
    }

    public static void Return(T obj, Action<T> resetAction = null)
    {
        resetAction?.Invoke(obj);
        _pool.Enqueue(obj);
    }
}
