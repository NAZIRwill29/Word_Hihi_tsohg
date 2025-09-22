using UnityEngine;

//lockpick, barrier
public class MiniGameInteractableOnCollsionAndKey : MiniGameInteractable
{
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected TriggerStay m_TriggerStay;
    [SerializeField] protected TriggerExit m_TriggerExit;
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    [SerializeField] protected NameDataFlyweight m_InputNameDataFlyweight;

    protected virtual void OnEnable()
    {
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;

        if (m_InputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.AddListener(StartMiniGame);
        }
    }
    protected virtual void OnDisable()
    {
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;

        if (m_InputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.RemoveListener(StartMiniGame);
        }
    }

    protected virtual void TriggerEffect(Collision2D other)
    {
        TriggerEffect(other.collider);
    }

    protected virtual void TriggerEffect(Collider2D other)
    {
        IsActive = true;
    }

    protected virtual void TriggerExit(Collider2D other)
    {
        IsActive = false;
    }
}
