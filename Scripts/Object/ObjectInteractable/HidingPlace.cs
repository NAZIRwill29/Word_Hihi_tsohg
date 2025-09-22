using UnityEngine;
using UnityEngine.Events;

public class HidingPlace : ObjectInteractable
{
    [SerializeField] protected InputManagerSO m_MiniGameInputManagerSO;
    private Transform m_Transform;
    [SerializeField] private ObjectLight m_ObjectLight;
    [SerializeField] protected Vector3 m_AddCameraOffset;
    public UnityEvent<bool> OnInteract;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_MiniGameInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_MiniGameInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.AddListener(InteractInPhaseOne);
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (m_MiniGameInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_MiniGameInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.AddListener(InteractInPhaseOne);
        }
    }

    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    protected override void TriggerEffect(Collision2D other)
    {
        TriggerEffect(other.collider);
    }

    protected override void TriggerEffect(Collider2D other)
    {
        if (m_PhaseNum != 0) return;
        m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseZeroNameData.Name, true);
        m_IsCheckInteractKey = true;
    }

    protected override void TriggerExit(Collider2D other)
    {
        if (m_PhaseNum != 0) return;
        m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseZeroNameData.Name, false);
        m_IsCheckInteractKey = false;
    }

    protected override void InteractInPhaseOne()
    {
        //unhide
        GameManager.Instance.MiniGameManager.ExitMiniGame(true);
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseZeroNameData.Name);
        GameManager.Instance.HidingMechanic.UnHide();
        m_PhaseNum = 0;
        OnInteract.Invoke(false);
    }

    protected override void Interact()
    {
        if (!m_IsCheckInteractKey) return;
        //hide
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseOneNameData.Name);
        GameManager.Instance.HidingMechanic.StartHide(this, m_ObjectLight, m_AddCameraOffset, m_Transform);
        m_PhaseNum = 1;
        OnInteract.Invoke(true);
    }

    public void ForceInteractInPhaseOne()
    {
        InteractInPhaseOne();
    }
}
