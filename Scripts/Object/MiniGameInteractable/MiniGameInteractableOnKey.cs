using UnityEngine;

//dispel
public class MiniGameInteractableOnKey : MiniGameInteractable
{
    [SerializeField] protected InputManagerSO m_InputManagerSO;
    [SerializeField] protected NameDataFlyweight m_InputNameDataFlyweight;

    protected virtual void OnEnable()
    {
        if (m_InputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.AddListener(StartMiniGame);
        }
    }
    protected virtual void OnDisable()
    {
        if (m_InputManagerSO && m_InputNameDataFlyweight)
        {
            if (m_InputManagerSO.InputDetails.TryGetValue(x => x.Name == m_InputNameDataFlyweight.Name, out InputDetail inputDetail))
                inputDetail.Action.RemoveListener(StartMiniGame);
        }
    }
}
