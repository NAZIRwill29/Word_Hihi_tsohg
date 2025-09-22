using UnityEngine;
using UnityEngine.Pool;

public class ObjParticleSystem : MonoBehaviour, IPoolable
{
    private IObjectPool<IPoolable> m_ObjectPool;
    public IObjectPool<IPoolable> ObjectPool
    {
        set => m_ObjectPool = value;
    }
    public ParticleSystem m_ParticleSystem;
    //public string name;
    // Cooldown time between particle system plays.
    public float Cooldown = 1f;
    public float TimeToNextPlay { get; set; }
    public bool IsOneAtATime;

    public virtual void Initialize(IObjectPool<IPoolable> pool)
    {
        ObjectPool = pool;
        TimeToNextPlay = Cooldown;
    }

    void Update()
    {
        TimeToNextPlay -= Time.deltaTime;
        if (TimeToNextPlay < 0)
            Deactivate();
    }

    public void Deactivate()
    {
        TimeToNextPlay = Cooldown;
        if (m_ObjectPool != null)
        {
            m_ObjectPool.Release(this);
        }
        else
        {
            Debug.LogWarning("ObjParticleSystem: Object pool is not assigned. Destroying the projectile.");
            Destroy(gameObject);
        }
    }
}