using UnityEngine;

//hide
public class MiniGameInteractableOnCollision : MiniGameInteractable
{
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected TriggerStay m_TriggerStay;
    [SerializeField] protected TriggerExit m_TriggerExit;

    protected virtual void OnEnable()
    {
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;
    }
    protected virtual void OnDisable()
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
        if (IsActive)
        {
            Collider2D = other;
            StartMiniGame();
        }
    }

    protected virtual void TriggerExit(Collider2D other)
    {
    }

    //trigger in hiding place unityevent
    public void ChangeActive(bool isTrue)
    {
        IsActive = isTrue;
        if (!IsActive)
            GameManager.Instance.MiniGameManager.ExitMiniGame(true);
    }
}
