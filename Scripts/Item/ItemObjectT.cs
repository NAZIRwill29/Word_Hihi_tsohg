using UnityEngine;
using UnityEngine.Events;

public class ItemObjectT : ObjectT, IActivable
{
    public NatureElement NatureElement;
    public event UnityAction<Collider2D> OnTriggerEffect;
    public bool IsActiveState { get; set; }
    public bool IsInActiveState { get; set; }
    public bool IsMoveState { get; set; }
    public bool InProgressInActiveState { get; set; }
    [SerializeField] private bool m_IsActive;
    public bool IsActive
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }

    protected virtual void TriggerEffect(Collider2D collider)
    {
        OnTriggerEffect?.Invoke(collider);
    }

    protected virtual void TriggerEffect(Collision2D collision)
    {
        OnTriggerEffect?.Invoke(collision.collider);
    }

    public virtual void Deactivate()
    {
        //Debug.Log("Deactivate " + this);
        IsInActiveState = true;
    }

    public void Activate(bool isTrue)
    {
        m_IsActive = isTrue;
    }
}
