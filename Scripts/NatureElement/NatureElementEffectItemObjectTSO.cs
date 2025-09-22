using System;
using System.Linq;
//using System.Reflection;
using UnityEngine;

//system.reflection

[CreateAssetMenu(fileName = "NatureElementEffectItemObjectT", menuName = "NatureElementEffect/NatureElementEffectItemObjectT")]
public class NatureElementEffectItemObjectTSO : NatureElementEffectSO
{
    [SerializeField] protected NameDataFlyweight m_StatsDataNameDataFlyweight;
    //[SerializeField] protected string m_StatsDataName = "HealthData";

    public virtual void BenefitEffect(ItemObjectT itemObjectT)
    {
        ApplyEffect(itemObjectT, m_BenefitNum);
        Debug.Log("NatureElementEffectObjectStatTriggerSO BenefitEffect");
    }

    public virtual void SupportEffect(ItemObjectT itemObjectT)
    {
        ApplyEffect(itemObjectT, m_SupportNum);
        Debug.Log("NatureElementEffectObjectStatTriggerSO SupportEffect");
    }

    public virtual void StrengthEffect(ItemObjectT itemObjectT)
    {
        ApplyEffect(itemObjectT, m_StrengthNum);
        Debug.Log("NatureElementEffectObjectStatTriggerSO StrengthEffect");
    }

    public virtual void WeaknessEffect(ItemObjectT itemObjectT)
    {
        ApplyEffect(itemObjectT, m_WeaknessNum);
        Debug.Log("NatureElementEffectObjectStatTriggerSO WeaknessEffect");
    }

    public virtual void ApplyEffect(ItemObjectT itemObjectT, float num)
    {
        ObjectStatTrigger objectStatTrigger = GetObjectStatTrigger(itemObjectT);
        if (objectStatTrigger == null) return;
        DataNumericalVariable dataNumVar = GetDataNumVar(objectStatTrigger);
        if (dataNumVar == null) return;
        dataNumVar.AddNumVariable += num;
        string extraText = num > 0 ? "+" : "-";
        if (objectStatTrigger != null)
            objectStatTrigger.ThingHappen(extraText + num);
    }

    private DataNumericalVariable GetDataNumVar(ObjectStatTrigger objectStatTrigger)
    {
        StatsData statsData = GetStatsData(objectStatTrigger);
        if (statsData == null) return null;

        return VariableFinder.GetVariableContainNameFromList(statsData.DataNumVars, m_NameDataFlyweight.Name);
    }

    private ObjectStatTrigger GetObjectStatTrigger(ObjectT objectT)
    {
        // Get the type from the string name
        Type type = Type.GetType(m_MonoBehaviourNameDataFlyweight.Name);
        if (type == null)
        {
            Debug.LogError($"Type {m_MonoBehaviourNameDataFlyweight.Name} not found!");
            return null;
        }

        // Get the component from the object
        Component component = objectT.GetComponent(type);
        if (component == null)
        {
            //Debug.LogError($"Component {m_MonoBehaviourNameDataFlyweight.Name} not found on {objectT.name}!");
            return null;
        }
        else
        {
            if (component is ObjectStatTrigger objectStatTrigger)
                return objectStatTrigger;
            else
                return null;
        }
    }

    private StatsData GetStatsData(ObjectStatTrigger objectStatTrigger)
    {
        if (objectStatTrigger == null)
        {
            Debug.LogError("objectStatTrigger is null!");
            return null;
        }

        if (objectStatTrigger is IStatsDataProvider provider)
        {
            return provider.GetStatsDataByName(m_StatsDataNameDataFlyweight.Name);
        }

        Debug.LogError($"Object {objectStatTrigger.GetType().Name} does not implement IStatsDataProvider!");
        return null;
    }

    // private StatsData GetStatsData(ObjectStatTrigger objectStatTrigger)
    // {
    //     if (objectStatTrigger == null)
    //     {
    //         Debug.LogError("objectStatTrigger is null!");
    //         return null;
    //     }

    //     // Use reflection to get the field or property by name
    //     FieldInfo fieldInfo = objectStatTrigger.GetType().GetField(m_StatsDataNameDataFlyweight.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     if (fieldInfo != null && typeof(StatsData).IsAssignableFrom(fieldInfo.FieldType))
    //     {
    //         return fieldInfo.GetValue(objectStatTrigger) as StatsData;
    //     }

    //     PropertyInfo propertyInfo = objectStatTrigger.GetType().GetProperty(m_StatsDataNameDataFlyweight.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     if (propertyInfo != null && typeof(StatsData).IsAssignableFrom(propertyInfo.PropertyType))
    //     {
    //         return propertyInfo.GetValue(objectStatTrigger) as StatsData;
    //     }

    //     //Debug.LogError($"Variable '{m_StatsDataNameDataFlyweight.Name}' not found in {objectStatTrigger.GetType().Name} or is not of type StatsData!");
    //     return null;
    // }
}
