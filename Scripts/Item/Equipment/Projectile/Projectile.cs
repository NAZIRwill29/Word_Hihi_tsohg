using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ItemObjectT, IPoolable
{
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected ObjectStatTrigger[] m_ObjectStatTriggers;
    public ProjectileDataFlyweight ProjectileDataFlyweight;
    private IObjectPool<IPoolable> m_ObjectPool;
    public IObjectPool<IPoolable> ObjectPool
    {
        set => m_ObjectPool = value;
    }

    protected Rigidbody2D m_Rigidbody;
    //protected SpriteRenderer m_SpriteRenderer;
    [SerializeField] protected ProjectileDuration[] m_ProjectileDurations;
    public bool IsLaunch;
    protected Vector2 m_OriginPos;
    protected float m_Force = 10;
    [SerializeField] private ProjectileRuntimeSetSO RuntimeSet;

    protected void OnEnable()
    {
        if (RuntimeSet) RuntimeSet.Add(this);
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
    }
    protected void OnDisable()
    {
        if (RuntimeSet) RuntimeSet.Remove(this);
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
    }

    public virtual void Initialize(IObjectPool<IPoolable> pool)
    {
        Activate(true);
        ObjectPool = pool;
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            if (m_Rigidbody == null)
            {
                Debug.LogError("Projectile: Rigidbody2D is missing!");
            }
        }
        //m_SpriteRenderer = GetComponent<SpriteRenderer>();

        if (m_ProjectileDurations == null || m_ProjectileDurations.Length == 0)
        {
            m_ProjectileDurations = GetComponents<ProjectileDuration>();
        }

        if (m_ObjectStatTriggers == null || m_ObjectStatTriggers.Length == 0)
        {
            m_ObjectStatTriggers = GetComponents<ObjectStatTrigger>();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        if (m_ProjectileDurations == null || m_ProjectileDurations.Length == 0)
        {
            m_ProjectileDurations = GetComponents<ProjectileDuration>();
        }
        if (m_ObjectStatTriggers == null || m_ObjectStatTriggers.Length == 0)
        {
            m_ObjectStatTriggers = GetComponents<ObjectStatTrigger>();
        }
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!IsActive) return;
        base.Update();
        // Future updates or behaviors can be added here.
    }

    // Launch with target
    public virtual void Launch(Vector2 position, Transform target, float velocity)
    {
        Vector2 direction = (Vector2)target.position - position;
        Launch(position, direction, velocity);
    }

    //launch with rotation of nuzzle
    public void Launch(Vector3 position, Quaternion rotation, float velocity)
    {
        //Debug.Log("Launch");
        Activate();
        transform.SetPositionAndRotation(position, rotation);
        Move(velocity);

        // Use Rigidbody to apply force (2D physics uses right direction for rotation)
        m_Rigidbody.AddForce(transform.right * m_Force, ForceMode2D.Impulse);
        // Use cached Rigidbody to apply force
        //m_Rigidbody.AddForce(transform.forward * m_Force);
    }
    //launch with direction
    public virtual void Launch(Vector2 position, Vector2 direction, float velocity)
    {
        //Debug.Log("Launch");
        Activate();
        transform.position = position;
        Move(velocity);
        // Calculate rotation angle based on direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        /*
        The function calls AddForce on the projectile’s Rigidbody component; the calculation for this here is 
        the direction variable multiplied by the force variable.
        When the force is applied to the Rigidbody component, the physics engine will apply that force and 
        direction to move the Projectile GameObject every frame.
        */
        // Apply force in the direction of the rotation
        m_Rigidbody.AddForce(direction.normalized * m_Force, ForceMode2D.Impulse);
        IsLaunch = true;

        foreach (var item in m_ProjectileDurations)
        {
            item.Launch();
        }
        m_OriginPos = position;
    }

    protected void Move(float velocity)
    {
        IsMoveState = true;
        m_Force = velocity * ProjectileDataFlyweight.ForceMultiFactor;
    }

    protected void StopMove()
    {
        IsMoveState = false;
        //m_SpriteRenderer.enabled = false;
        m_Rigidbody.linearVelocity = Vector2.zero;
        m_Rigidbody.angularVelocity = 0;
    }

    protected void Activate()
    {
        foreach (var item in m_ObjectStatTriggers)
        {
            item.IsActive = true;
            item.RevertStatsData();
        }
        Activate(true);

        //Animator.SetBool("Active", true);
        IsActiveState = true;
        IsAlive = true;
        ObjectHealth.IsDie = false;
        //HealthData.Revert();
    }

    protected override void TriggerEffect(Collider2D collider)
    {
        base.TriggerEffect(collider);
        StopMove();
        ObjectHealth.IsDie = true;
        IsAlive = false;
        //Deactivate();
    }

    protected override void TriggerEffect(Collision2D collision)
    {
        base.TriggerEffect(collision);
        StopMove();
        ObjectHealth.IsDie = true;
        IsAlive = false;
        //Deactivate();
    }

    public override void Deactivate()
    {
        Debug.Log("Deactivate " + this);
        IsInActiveState = true;
        foreach (var item in m_ObjectStatTriggers)
        {
            item.IsActive = false;
            item.RevertStatsData();
        }
        InProgressInActiveState = true;
        StartCoroutine(DeactivateDelay());
    }

    private IEnumerator DeactivateDelay()
    {
        yield return new WaitUntil(() => !InProgressInActiveState);
        Debug.Log("DeactivateDelay " + this);
        Activate(false);

        if (m_ObjectPool != null)
        {
            m_ObjectPool.Release(this);
            Debug.Log("Release " + this);
        }
        else
        {
            Debug.LogWarning("Projectile: Object pool is not assigned. Destroying the projectile.");
            Destroy(gameObject);
        }
    }
}
