using UnityEngine;
using UnityEngine.Pool;

public class PooledObject : MonoBehaviour, IPoolable
{
    private IObjectPool<IPoolable> m_ObjectPool;
    public IObjectPool<IPoolable> ObjectPool
    {
        set => m_ObjectPool = value;
    }

    public void Deactivate()
    {
        m_ObjectPool.Release(this);
    }

    public void Initialize(IObjectPool<IPoolable> pool)
    {
        ObjectPool = pool;
    }
}