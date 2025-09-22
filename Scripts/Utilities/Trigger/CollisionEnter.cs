using UnityEngine;
using UnityEngine.Events;

public class CollisionEnter : ColliderThing
{
    public event UnityAction<Collision2D> OnEnter2D;
    public event UnityAction<Collision> OnEnter;

    private void TryInvoke(Collision other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other.collider, m_StatTriggerFlyweightData.InteractLayers))
            OnEnter?.Invoke(other);
    }

    private void TryInvoke(Collision2D other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other.collider, m_StatTriggerFlyweightData.InteractLayers))
            OnEnter2D?.Invoke(other);
    }

    private void OnCollisionEnter2D(Collision2D other) => TryInvoke(other);
    private void OnCollisionEnter(Collision other) => TryInvoke(other);

    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other.collider, m_StatTriggerFlyweightData.InteractLayers))
    //     {
    //         OnEnter2D?.Invoke(other);
    //     }
    // }

    // void OnCollisionEnter(Collision other)
    // {
    //     if (!m_IsNoNeedActivation)
    //     {
    //         if (m_EnvironmentObject != null && !m_EnvironmentObject.IsActive) return;
    //         if (m_ItemObjectT != null && !m_ItemObjectT.IsActive) return;
    //     }
    //     if (m_StatTriggerFlyweightData == null) return;

    //     if (LayerInteraction.IsLayerInteractable(other.collider, m_StatTriggerFlyweightData.InteractLayers))
    //     {
    //         OnEnter?.Invoke(other);
    //     }
    // }
}
