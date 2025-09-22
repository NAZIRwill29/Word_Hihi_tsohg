using UnityEngine;
using UnityEngine.Events;

public class TriggerEnter : ColliderThing
{
    public event UnityAction<Collider2D> OnEnter2D;
    public event UnityAction<Collider> OnEnter;

    private void TryInvoke(Collider other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnEnter?.Invoke(other);
    }

    private void TryInvoke(Collider2D other)
    {
        if (!CanActivate()) return;

        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            OnEnter2D?.Invoke(other);
    }

    private void OnTriggerEnter2D(Collider2D other) => TryInvoke(other);
    private void OnTriggerEnter(Collider other) => TryInvoke(other);
}
