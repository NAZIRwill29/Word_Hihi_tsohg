using UnityEngine;
using UnityEngine.Events;

public class ColliderThing : MonoBehaviour
{
    [SerializeField] protected StatTriggerFlyweight m_StatTriggerFlyweightData;
    protected EnvironmentObject m_EnvironmentObject;
    protected ItemObjectT m_ItemObjectT;
    [SerializeField] protected bool m_IsNoNeedActivation;

    protected virtual void Awake()
    {
        if (TryGetComponent(out EnvironmentObject environmentObject))
            m_EnvironmentObject = environmentObject;
        if (TryGetComponent(out ItemObjectT itemObjectT))
            m_ItemObjectT = itemObjectT;
    }

    protected bool CanActivate()
    {
        if (!m_IsNoNeedActivation)
        {
            if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive)
                return false;

            if (m_ItemObjectT != null && !m_ItemObjectT.IsActive)
                return false;
        }

        return m_StatTriggerFlyweightData != null;
    }
}
