using UnityEngine;
using UnityEngine.Pool;

public class ItemInteractable : ObjectInteractable, IPoolable
{
    [SerializeField] protected NameDataFlyweight m_PositiveInput;
    [SerializeField] protected NameDataFlyweight m_NegativeInput;
    [SerializeField] private Item m_Item;
    [SerializeField] protected NameDataFlyweight SoundNameData;
    [SerializeField] protected NameDataFlyweight FXNameData;
    private IObjectPool<IPoolable> m_ObjectPool;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_NormalInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_PositiveInput.Name, out InputDetail takeInputDetail))
                takeInputDetail.Action.AddListener(Take);
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_NegativeInput.Name, out InputDetail dropInputDetail))
                dropInputDetail.Action.AddListener(Drop);
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (m_NormalInputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_PositiveInput.Name, out InputDetail takeInputDetail))
                takeInputDetail.Action.RemoveListener(Take);
            if (m_NormalInputManagerSO.InputDetails.TryGetValue(x => x.Name == m_NegativeInput.Name, out InputDetail dropInputDetail))
                dropInputDetail.Action.RemoveListener(Drop);
        }
    }

    public void Initialize(IObjectPool<IPoolable> pool)
    {
        m_ObjectPool = pool;
    }

    public void Show(GameObject gameObject)
    {
        m_IsActive = true;
        transform.position = gameObject.transform.position;
    }

    public void Deactivate()
    {
        m_IsActive = false;

        if (m_ObjectPool != null)
        {
            m_ObjectPool.Release(this);
        }
        else
        {
            Debug.LogWarning("Floating Text: Object pool is not assigned. Destroying the Floating Text.");
            Destroy(gameObject);
        }
    }

    protected override void Interact()
    {
        if (!m_IsCheckInteractKey) return;
        if (!m_IsActive) return;
        //do interact
        //m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseZeroNameData.Name, false);
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseZeroNameData.Name);
        m_InteractManagerSO.ShowItemPopUp(m_Item);
        m_PhaseNum = 1;
        m_IsActive = false;
    }

    protected void Take()
    {
        if (m_PhaseNum != 1) return;
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseOneNameData.Name);
        m_InteractManagerSO.OnCanInteract.Invoke(m_InteractPhaseOneNameData.Name, false);
        m_InteractManagerSO.OnClosePopUp.Invoke();

        string soundName = SoundNameData != null ? SoundNameData.Name : "";
        string fxName = FXNameData != null ? FXNameData.Name : "";

        m_ObjectT.ThingHappen(new() { SoundName = soundName, FXName = fxName });

        // Notify any listeners through the event channel
        GameEvents.CollectibleCollected();
        GameEvents.ItemCollected(m_ObjectT.ObjectId, m_Item);

        Destroy(gameObject);
    }

    protected void Drop()
    {
        if (m_PhaseNum != 1) return;

        InteractInPhaseOne();
        m_IsActive = true;
    }

    protected override void InteractInPhaseOne()
    {
        m_InteractManagerSO.OnInteract.Invoke(m_InteractPhaseOneNameData.Name);
        m_InteractManagerSO.OnClosePopUp.Invoke();
        m_PhaseNum = 0;
    }
}
