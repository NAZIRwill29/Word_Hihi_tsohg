using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ObjectPool", menuName = "Manager/ObjectPool")]
public class ObjectPoolScriptable : ScriptableObject
{
    private GameObject m_PoolContainer;

    [Tooltip("Throw errors if we try to release an item that is already in the pool")]
    [SerializeField] protected bool m_CollectionCheck = true;

    [SerializeField] protected List<ObjectPooledPrefab> m_ObjectPooledPrefabs;

    protected Dictionary<string, IObjectPool<IPoolable>> m_ObjectPools = new Dictionary<string, IObjectPool<IPoolable>>();

    public void Initialize(GameObject poolContainer)
    {
        if (poolContainer == null)
        {
            Debug.LogError("ObjectPool: Pool container is null!");
            return;
        }

        m_PoolContainer = poolContainer;

        if (m_ObjectPooledPrefabs == null || m_ObjectPooledPrefabs.Count == 0)
        {
            Debug.LogError("ObjectPool: No object prefabs assigned.");
            return;
        }

        m_ObjectPools.Clear();

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
        if (m_PoolContainer == null)
        {
            Debug.LogError("ObjectPool: Pool container is null! Cannot create pooled object.");
            return null;
        }

        GameObject instance = Instantiate(pooledPrefab.m_ObjectPrefab, m_PoolContainer.transform);
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
}
