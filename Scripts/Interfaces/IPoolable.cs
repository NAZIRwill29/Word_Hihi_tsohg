using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable
{
    void Initialize(IObjectPool<IPoolable> pool);
    void Deactivate();
}