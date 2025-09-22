using UnityEngine;

public class CombatStarterCollision : CombatStarter
{
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected TriggerStay m_TriggerStay;
    [SerializeField] protected TriggerExit m_TriggerExit;
    public bool IsActive = true;

    protected void OnEnable()
    {
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;
    }
    protected void OnDisable()
    {
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;
    }

    protected virtual void TriggerEffect(Collision2D other)
    {
        TriggerEffect(other.collider);
    }

    protected virtual void TriggerEffect(Collider2D other)
    {
        if (!IsActive) return;
        if (GameManager.Instance.PlayModeManager.PlayModeCurrentName == "Combat") return;

        Ghost ghost = other.GetComponent<Ghost>();
        if (!ghost)
            ghost = other.GetComponentInParent<Ghost>();
        if (ghost)
            StartCombat(ghost);
    }

    protected virtual void TriggerExit(Collider2D other)
    {
        if (!IsActive) return;
    }
}
