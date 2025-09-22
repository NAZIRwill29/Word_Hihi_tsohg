using UnityEngine;

public class StatTriggerFlyweight : ScriptableObject
{
    [SerializeField] private NameDataFlyweight m_ReceiverNameDataFlyweight;
    public string Receiver
    {
        get => m_ReceiverNameDataFlyweight != null ? m_ReceiverNameDataFlyweight.Name : string.Empty;
    }
    public LayerMask InteractLayers;
}
