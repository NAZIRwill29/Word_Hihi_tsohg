using UnityEngine;
using UnityEngine.Events;

public class TriggerExit : ColliderThing
{
    public event UnityAction<Collider2D> OnExit2D;
    public event UnityAction<Collider> OnExit;

    private void TryInvoke(Collider other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnExit?.Invoke(other);
    }

    private void TryInvoke(Collider2D other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnExit2D?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other) => TryInvoke(other);
    private void OnTriggerExit(Collider other) => TryInvoke(other);

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
    //         OnExit2D?.Invoke(other);
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
    //         OnExit?.Invoke(other);
    // }
}
