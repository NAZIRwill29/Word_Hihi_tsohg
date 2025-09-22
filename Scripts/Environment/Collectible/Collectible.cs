using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour, ICheckerUser, IActivable
{
    public Dictionary<string, Type> TypeCache { get; set; } = new(); // Cached type lookup
    public bool IsCacheInitialized { get; set; } = false;
    [SerializeField] protected StatTriggerFlyweight m_StatTriggerFlyweightData;
    public StatTriggerFlyweight StatTriggerFlyweight { get => m_StatTriggerFlyweightData; }
    [SerializeField] protected NameDataFlyweight SoundNameData;
    [SerializeField] protected NameDataFlyweight FXNameData;
    [SerializeField] private CollectibleRuntimeSetSO RuntimeSet;
    [SerializeField] private bool m_IsActive;
    public bool IsActive
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }

    public string SoundName => SoundNameData != null ? SoundNameData.Name : string.Empty;

    protected void OnEnable()
    {
        if (RuntimeSet) RuntimeSet.Add(this);
    }

    protected void OnDisable()
    {
        if (RuntimeSet) RuntimeSet.Remove(this);
    }

    public void Activate(bool isTrue)
    {
        m_IsActive = isTrue;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActive) return;
        if (!LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers)) return;


        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Checker '{m_StatTriggerFlyweightData.Receiver}' is null or missing ObjectT on {other.name}.");
            return;
        }

        ObjectT objectT = receiver.ObjectT;
        if (objectT)
            ThingHappen(objectT);
    }

    protected virtual void ThingHappen(ObjectT objectT)
    {
        string soundName = SoundNameData != null ? SoundNameData.Name : "";
        string fxName = FXNameData != null ? FXNameData.Name : "";

        objectT.ThingHappen(new() { SoundName = soundName, FXName = fxName });

        // Notify any listeners through the event channel
        GameEvents.CollectibleCollected();

        Destroy(gameObject);
    }
}
