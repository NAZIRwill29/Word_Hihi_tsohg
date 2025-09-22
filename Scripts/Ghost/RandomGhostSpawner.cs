using System.Collections.Generic;
using UnityEngine;

//UNUSED
public class RandomGhostSpawner : MonoBehaviour
{
    private float m_SpawnedCooldown;
    [SerializeField] private float m_SpawnedTimeMin, m_SpawnedTimeMax;
    [SerializeField] private List<NameDataFlyweight> m_GhostNameDatas;
    [SerializeField] private List<Vector2> m_ListSpawnedPos;
    private bool m_IsCanSpawned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_SpawnedCooldown = Random.Range(m_SpawnedTimeMin, m_SpawnedTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsCanSpawned)
            return;

        m_SpawnedCooldown -= Time.deltaTime;
        if (m_SpawnedCooldown < 0)
        {
            m_SpawnedCooldown = Random.Range(m_SpawnedTimeMin, m_SpawnedTimeMax);

            //spawned
            SpawnedRandom();
        }
    }

    public void SpawnedRandom()
    {
        Spawned(
            m_GhostNameDatas[Random.Range(0, m_GhostNameDatas.Count)].Name,
            m_ListSpawnedPos[Random.Range(0, m_ListSpawnedPos.Count)]
        );
    }

    public void Spawned(string name, Vector2 position)
    {
        var pooledObject = GameManager.Instance.ObjectPool.GetPooledObject(name);
        if (pooledObject == null)
        {
            Debug.LogWarning($"No pooled object found for {name}");
            return;
        }
        if (pooledObject is Ghost ghost)
        {
            //ghost.Activate(position);
        }
    }
}
