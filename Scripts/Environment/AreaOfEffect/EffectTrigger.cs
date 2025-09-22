using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    [Tooltip("The AreaOfEffect triggered with colliding with this component")]
    [SerializeField] AreaOfEffect m_Effect;
    [Tooltip("The minimum time in seconds between triggers")]
    [SerializeField] float m_Cooldown = 2f;

    float m_LastEffectTime = -1;
    [SerializeField] protected StatTriggerFlyweight m_StatTriggerFlyweightData;

    private void OnTriggerEnter(Collider other)
    {
        PlayEffect(other);

        if (m_Effect != null && LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            m_Effect.ShowAreaText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            PlayEffect(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Effect != null && LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers))
            m_Effect.ShowLabelText(string.Empty);
    }

    private void PlayEffect(Collider other)
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
