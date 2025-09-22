using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectStatsManager : MonoBehaviour, IObjectStatProvider
{
    public ObjectT ObjectT { get; set; }
    public ObjectStat[] ObjectStats { get; private set; }
    private Dictionary<string, ObjectStat> m_ObjectStatDictionary = new();
    [SerializeField] private Receiver[] m_Receivers;
    public Dictionary<string, Receiver> ReceiversDictionary = new();
    public event UnityAction<Collider2D> OnTrigger;

    private void Awake()
    {
        ObjectT = GetComponent<ObjectT>();
        if (ObjectT == null)
        {
            Debug.LogError($"{nameof(ObjectStatsManager)}: Missing {nameof(ObjectT)} component on {gameObject.name}!");
            return;
        }

        ObjectStats = GetComponentsInChildren<ObjectStat>() ?? new ObjectStat[0];  // Ensure ObjectStats is never null

        foreach (var objectStat in ObjectStats)
        {
            if (objectStat != null && objectStat.StatsData.StatDataNameDataFlyweight != null)
            {
                m_ObjectStatDictionary[objectStat.StatsData.StatDataNameDataFlyweight.Name] = objectStat;
            }
        }

        foreach (var receiver in m_Receivers)
        {
            ReceiversDictionary[receiver.Name] = receiver;
        }
    }

    public ObjectStat GetObjectStatByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError($"{nameof(ObjectStatsManager)}: Provided name is null or empty.");
            return null;
        }

        if (m_ObjectStatDictionary.Count == 0)
        {
            Debug.LogWarning($"{nameof(ObjectStatsManager)}: ObjectStat dictionary is empty. Ensure ObjectStats are properly assigned.");
            return null;
        }

        return m_ObjectStatDictionary.TryGetValue(name, out var objectStat) ? objectStat : null;
    }

    public void Triggered(Collider2D other, object effectData, bool inZone)
    {
        //(2) call ChangeVariable in CharacterStat (data)
        //ex: DefenseZone - call ChangeVariable(DefenseEffectData) in DefenseEffect
        //Debug.Log($"Triggered called with effectData: {effectData}");
        foreach (var objectStat in ObjectStats)
        {
            // if (effectData is HealthData healthData)
            // {
            //     DataNumericalVariable source = VariableFinder.GetVariableContainNameFromList(healthData.DataNumVars, "Health");
            //     if (source != null)
            //         Debug.Log("AddNumVariable 2 " + source.AddNumVariable);
            // }
            if (objectStat.StatsData.StatDataNameDataFlyweight.Name == effectData.ToString())
            {
                objectStat.ChangeAllVariable(effectData, inZone);
            }
            //characterStat.Triggered(characterStat, effectData);
        }
        OnTrigger?.Invoke(other);
    }

    public void ExitTriggered(object effectData)
    {
        //(2) call ChangeVariable in CharacterStat (data)
        //ex: DefenseZone - call ChangeVariable(DefenseEffectData) in DefenseEffect
        //Debug.Log($"Triggered called with effectData: {effectData}");
        foreach (var objectStat in ObjectStats)
        {
            if (objectStat.StatsData.StatDataNameDataFlyweight.Name == effectData.ToString())
            {
                objectStat.ExitTriggered();
            }
            //effect.Triggered(characterStat, effectData);
        }
    }
}
