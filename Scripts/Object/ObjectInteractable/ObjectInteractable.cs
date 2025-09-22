using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField] protected CollisionEnter m_CollisionEnter;
    [SerializeField] protected TriggerEnter m_TriggerEnter;
    [SerializeField] protected TriggerStay m_TriggerStay;
    [SerializeField] protected TriggerExit m_TriggerExit;
    [SerializeField] protected InputManagerSO m_NormalInputManagerSO;
    [SerializeField] protected NameDataFlyweight m_InputNameDataFlyweight;
    protected bool m_IsCheckInteractKey;
    [SerializeField] protected NameDataFlyweight m_InteractPhaseZeroNameData;
    [SerializeField] protected NameDataFlyweight m_InteractPhaseOneNameData;
    protected int m_PhaseNum;
    [SerializeField] private InteractableRunTimeSetSO m_RuntimeSet;
    [SerializeField] protected InteractManagerSO m_InteractManagerSO;
    protected ObjectT m_ObjectT;
    [SerializeField] protected bool m_IsActive = true;

    protected virtual void OnEnable()
    {
        if (m_RuntimeSet) m_RuntimeSet.Add(this);
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D += TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D += TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D += TriggerExit;

        if (m_NormalInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.AddListener(Interact);
        }
    }
    protected virtual void OnDisable()
    {
        if (m_RuntimeSet) m_RuntimeSet.Remove(this);
        if (m_CollisionEnter) m_CollisionEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerEnter) m_TriggerEnter.OnEnter2D -= TriggerEffect;
        if (m_TriggerStay) m_TriggerStay.OnStay2D -= TriggerEffect;
        if (m_TriggerExit) m_TriggerExit.OnExit2D -= TriggerExit;

        if (m_NormalInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.RemoveListener(Interact);
        }
    }

    protected virtual void TriggerEffect(Collision2D other)
    {
        TriggerEffect(other.collider);
    }

    protected virtual void TriggerEffect(Collider2D other)
    {
        m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseZeroNameData.Name, true);
        m_IsCheckInteractKey = true;
        m_ObjectT = other.GetComponentInParent<ObjectT>();
        //GameManager.Instance.NormalSystem.ObjectInteractable = this;
    }

    protected virtual void TriggerExit(Collider2D other)
    {
        m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseZeroNameData.Name, false);
        m_IsCheckInteractKey = false;
        m_ObjectT = other.GetComponentInParent<ObjectT>();
    }

    protected virtual void InteractInPhaseOne()
    {
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseZeroNameData.Name);
        m_PhaseNum = 0;
    }

    protected virtual void Interact()
    {
        if (!m_IsCheckInteractKey) return;
        //do interact
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseOneNameData.Name);
        m_PhaseNum = 1;
    }
}
