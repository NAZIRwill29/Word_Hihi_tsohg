using UnityEngine;
using UnityEngine.Events;

public class ObjectStatTrigger : StatsTrigger
{
    protected ObjectT m_ObjectT;
    //[SerializeField] protected bool m_IsActiveOnStart;
    public bool IsActive;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected TriggerExit m_TriggerExit;
    public event UnityAction<Collider2D> OnTriggerEffect, OnTriggerExit;
    [SerializeField] protected ThingHappenTriggerSO m_ThingHappenTriggerSO;
    private ThingHappenData m_RuntimeThingHappenData = new();


    protected virtual void Awake()
    {
        m_ObjectT = GetComponent<ObjectT>();
    }

    protected virtual void OnEnable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;
    }
    protected virtual void OnDisable()
    {
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;
    }

    protected virtual void Start()
    {
        if (m_StatTriggerFlyweightData is ObjectStatTriggerFlyweight objectStatTriggerFlyweight)
            IsActive = objectStatTriggerFlyweight.IsActiveOnStart;
    }

    protected override void TriggerEffect(Collider2D other)
    {
        if (!IsActive) return;
        //if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers)) 
        ProcessTrigger(other);
    }

    protected override void TriggerEffect(Collision2D collision)
    {
        if (!IsActive) return;
        //if (LayerInteraction.IsLayerInteractable(collision.collider, m_StatTriggerFlyweightData.InteractLayers)) 
        ProcessTrigger(collision.collider);
    }

    protected override void ProcessTrigger(Collider2D other)
    {
        if (!IsActive) return;

        OnTriggerEffect?.Invoke(other);

        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver != null && receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            Debug.LogWarning($"Checker of type {m_StatTriggerFlyweightData.Receiver} is null or missing ObjectStatsManager!");
            return;
        }
        IsActive = false;

        var effectData = CreateEffectData();
        if (effectData != null)
        {
            receiver.ObjectStatsManager.Triggered(other, effectData, true);
        }
        else
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Effect data is null for {other.name}.");
        }
    }

    protected override void ProcessExit(Collider2D other)
    {
        OnTriggerExit?.Invoke(other);

        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            //Debug.LogWarning($"[{nameof(StatsTrigger)}] Checker '{m_StatTriggerFlyweightData.Receiver}' is null or missing ObjectStatsManager on {other.name}.");
            return;
        }

        var effectData = CreateEffectData();
        if (effectData != null)
        {
            receiver.ObjectStatsManager.ExitTriggered(effectData);
        }
        else
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Effect data is null for {other.name}.");
        }
    }

    public virtual void RevertStatsData()
    {
    }

    public virtual void ThingHappen(string extraText = null)
    {
        //Debug.Log("ThingHappen " + m_ObjectT + " / " + extraText);
        if (m_ThingHappenTriggerSO)
            m_ThingHappenTriggerSO.ObjecTThingHappen(m_ObjectT, m_RuntimeThingHappenData, extraText);
    }
}
