using UnityEngine;
using UnityEngine.Events;

public class TriggerStay : ColliderThing
{
    public event UnityAction<Collider2D> OnStay2D;
    public event UnityAction<Collider> OnStay;

    private void TryInvoke(Collider other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnStay?.Invoke(other);
    }

    private void TryInvoke(Collider2D other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnStay2D?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other) => TryInvoke(other);
    private void OnTriggerStay(Collider other) => TryInvoke(other);

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
    //         OnStay2D?.Invoke(other);
    // }

    // void OnTriggerStay(Collider other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
    //         OnStay?.Invoke(other);
    // }
}
