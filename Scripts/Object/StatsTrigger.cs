using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using System.Collections.Generic;

public interface IStatsDataProvider
{
    StatsData GetStatsDataByName(string name);
}

//TODO()
public class StatsTrigger : MonoBehaviour, ICheckerUser, IStatsDataProvider
{
    public Dictionary<string, Type> TypeCache { get; set; } = new(); // Cached type lookup
    public bool IsCacheInitialized { get; set; } = false;
    [SerializeField] protected StatTriggerFlyweight m_StatTriggerFlyweightData;
    public StatTriggerFlyweight StatTriggerFlyweight { get => m_StatTriggerFlyweightData; }
    //[SerializeField] protected ThingHappenTriggerSO m_ThingHappenTriggerSO;

    #region Trigger Effect

    protected virtual void TriggerEffect(Collider2D other)
    {
        //if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers)) 
        ProcessTrigger(other);
    }

    protected virtual void TriggerEffect(Collision2D collision)
    {
        //if (LayerInteraction.IsLayerInteractable(collision.collider, m_StatTriggerFlyweightData.InteractLayers))
        ProcessTrigger(collision.collider);
    }

    protected virtual void ProcessTrigger(Collider2D other)
    {
        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            Debug.Log($"Checker of type {m_StatTriggerFlyweightData.Receiver} is null or missing ObjectStatsManager!");
            return;
        }

        var effectData = CreateEffectData();
        if (effectData != null)
        {
            // if (effectData is HealthData healthData)
            // {
            //     DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(healthData.DataNumVars, "Health");
            //     if (source != null)
            //         Debug.Log("AddNumVariable 1 " + source.AddNumVariable);
            // }
            receiver.ObjectStatsManager.Triggered(other, effectData, true);
        }
        else
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Effect data is null for {other.name}.");
        }
    }

    #endregion

    #region Trigger Exit

    protected void TriggerExit(Collider2D other)
    {
        //if (LayerInteraction.IsLayerInteractable(other, m_StatTriggerFlyweightData.InteractLayers)) 
        ProcessExit(other);
    }

    protected void TriggerExit(Collision2D collision)
    {
        //if (LayerInteraction.IsLayerInteractable(collision.collider, m_StatTriggerFlyweightData.InteractLayers)) 
        ProcessExit(collision.collider);
    }

    protected virtual void ProcessExit(Collider2D other)
    {
        Receiver receiver = other.GetComponent<Receiver>();
        if (receiver.Name != m_StatTriggerFlyweightData.Receiver)
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Checker '{m_StatTriggerFlyweightData.Receiver}' is null or missing ObjectStatsManager on {other.name}.");
            return;
        }

        var effectData = CreateEffectData();
        if (effectData != null)
        {
            receiver.ObjectStatsManager.ExitTriggered(effectData);
        }
        else
        {
            Debug.LogWarning($"[{nameof(StatsTrigger)}] Effect data is null for {other.name}.");
        }
    }

    #endregion

    #region Utility Methods

    public virtual StatsData GetStatsDataByName(string name)
    {
        return null;
    }

    // protected IReceiver GetCheckerComponent(Collider2D other)
    // {
    //     // Attempt to find the correct type from all loaded assemblies
    //     Type checkerType = Type.GetType(m_StatTriggerFlyweightData.Receiver) ??
    //                        AppDomain.CurrentDomain.GetAssemblies()
    //                        .SelectMany(a => a.GetTypes())
    //                        .FirstOrDefault(t => t.Name == m_StatTriggerFlyweightData.Receiver);

    //     if (checkerType == null)
    //     {
    //         Debug.LogWarning($"[{nameof(StatsTrigger)}] Checker type '{m_StatTriggerFlyweightData.Receiver}' could not be found.");
    //         return null;
    //     }

    //     var component = other.GetComponent(checkerType) as IReceiver;
    //     if (component == null)
    //     {
    //         Debug.LogWarning($"[{nameof(StatsTrigger)}] '{m_StatTriggerFlyweightData.Receiver}' component is missing on {other.name}.");
    //     }

    //     return component;
    // }

    public virtual object CreateEffectData()
    {
        return new StatsData();
    }

    #endregion
}
