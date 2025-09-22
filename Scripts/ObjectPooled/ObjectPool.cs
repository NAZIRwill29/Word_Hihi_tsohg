using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class ObjectPooledPrefab
{
    [SerializeField] private string m_Name;
    public NameDataFlyweight NameDataFlyweight;
    public string Name
    {
        get => NameDataFlyweight != null ? NameDataFlyweight.Name : String.Empty;
        set
        {
            if (NameDataFlyweight != null)
            {
                NameDataFlyweight.Name = value;
            }
            m_Name = value;  // Store locally in case NameDataFlyweight is null
        }
    }
    [Tooltip("Prefab for Object Pooled")]
    public GameObject m_ObjectPrefab; // Assumes the prefab implements IPoolable.
    [HideInInspector] public IPoolable m_PoolablePrefab;

    [Tooltip("Default pool size"), Range(0, 50)]
    public int m_DefaultCapacity = 20;

    [Tooltip("Pool can expand to this limit"), Range(50, 500)]
    public int m_MaxSize = 100;
}

public class ObjectPool : Singleton<ObjectPool>
{
    [Tooltip("Throw errors if we try to release an item that is already in the pool")]
    [SerializeField] protected bool m_CollectionCheck = true;

    [SerializeField] protected List<ObjectPooledPrefab> m_ObjectPooledPrefabs;

    protected Dictionary<string, IObjectPool<IPoolable>> m_ObjectPools;

    protected override void Awake()
    {
        base.Awake();
        if (m_ObjectPooledPrefabs == null || m_ObjectPooledPrefabs.Count == 0)
        {
            Debug.LogError("ObjectPool: No object prefabs assigned.");
            return;
        }

        m_ObjectPools = new Dictionary<string, IObjectPool<IPoolable>>();

        foreach (var pooledPrefab in m_ObjectPooledPrefabs)
        {
            if (pooledPrefab.m_ObjectPrefab == null)
            {
                Debug.LogError($"ObjectPool: Prefab for '{pooledPrefab.Name}' is null.");
                continue;
            }

            pooledPrefab.m_PoolablePrefab = pooledPrefab.m_ObjectPrefab.GetComponent<IPoolable>();
            if (pooledPrefab.m_PoolablePrefab == null)
            {
                Debug.LogError($"ObjectPool: The prefab '{pooledPrefab.m_ObjectPrefab.name}' does not implement IPoolable.");
                continue;
            }

            // Create the object pool for this prefab
            m_ObjectPools[pooledPrefab.Name] = new ObjectPool<IPoolable>(
                () => CreatePooledObject(pooledPrefab),
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject,
                m_CollectionCheck,
                pooledPrefab.m_DefaultCapacity,
                pooledPrefab.m_MaxSize
            );
        }
    }

    protected virtual IPoolable CreatePooledObject(ObjectPooledPrefab pooledPrefab)
    {
        GameObject instance = Instantiate(pooledPrefab.m_ObjectPrefab, gameObject.transform);
        IPoolable pooledObjectInstance = instance.GetComponent<IPoolable>();
        if (pooledObjectInstance == null)
        {
            Debug.LogError($"ObjectPool: The instantiated object '{instance.name}' does not implement IPoolable.");
            Destroy(instance);
            return null;
        }

        pooledObjectInstance.Initialize(m_ObjectPools[pooledPrefab.Name]);
        return pooledObjectInstance;
    }

    protected void OnReleaseToPool(IPoolable pooledObject)
    {
        if (pooledObject is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
        }
    }

    protected void OnGetFromPool(IPoolable pooledObject)
    {
        if (pooledObject is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(true);
        }
    }

    protected void OnDestroyPooledObject(IPoolable pooledObject)
    {
        if (pooledObject is MonoBehaviour monoBehaviour)
        {
            Destroy(monoBehaviour.gameObject);
        }
    }

    public IPoolable GetPooledObject(string prefabName)
    {
        if (m_ObjectPools.TryGetValue(prefabName, out var pool))
        {
            return pool.Get();
        }

        Debug.LogError($"ObjectPool: No pool found for prefab '{prefabName}'.");
        return null;
    }

    // protected virtual void FixedUpdate()
    // {
    //     // Placeholder for custom logic
    // }
}
