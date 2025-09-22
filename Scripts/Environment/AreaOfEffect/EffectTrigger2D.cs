using UnityEngine;

public class EffectTrigger2D : MonoBehaviour
{
    [Tooltip("The AreaOfEffect triggered with colliding with this component")]
    [SerializeField] AreaOfEffect m_Effect;
    [Tooltip("The minimum time in seconds between triggers")]
    [SerializeField] float m_Cooldown = 2f;

    float m_LastEffectTime = -1;
    [SerializeField] protected StatTriggerFlyweight m_StatTriggerFlyweightData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayEffect(other);

        if (m_Effect != null && LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            m_Effect.ShowAreaText();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            PlayEffect(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (m_Effect != null && LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            m_Effect.ShowLabelText(string.Empty);
    }

    private void PlayEffect(Collider2D other)
    {
        float nextEffectTime = m_LastEffectTime + m_Cooldown;

        // Check by tag 
        if (Time.time > nextEffectTime && LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
        {
            m_LastEffectTime = Time.time;

            // Trigger effect for player
            m_Effect.PlayEffect();
        }
    }
}
