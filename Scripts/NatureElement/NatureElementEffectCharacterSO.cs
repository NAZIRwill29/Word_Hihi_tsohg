using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "NatureElementEffectCharacter", menuName = "NatureElementEffect/NatureElementEffectCharacter")]
public class NatureElementEffectCharacterSO : NatureElementEffectSO
{
    //[SerializeField] protected string m_MonoBehaviourName = "ObjectStat";
    public virtual void BenefitEffect(Character character)
    {
        DataNumericalVariable dataNumVar = GetDataNumVar(character);
        if (dataNumVar != null) dataNumVar.NumVariable += m_BenefitNum;
        Debug.Log("NatureElementEffectObject BenefitEffect");
    }

    public virtual void SupportEffect(Character character)
    {
        DataNumericalVariable dataNumVar = GetDataNumVar(character);
        if (dataNumVar != null) dataNumVar.NumVariable -= m_SupportNum;
        Debug.Log("NatureElementEffectObject SupportEffect");
    }

    public virtual void StrengthEffect(Character character)
    {
        DataNumericalVariable dataNumVar = GetDataNumVar(character);
        if (dataNumVar != null) dataNumVar.NumVariable += m_StrengthNum;
        Debug.Log("NatureElementEffectObject StrengthEffect");
    }

    public virtual void WeaknessEffect(Character character)
    {
        DataNumericalVariable dataNumVar = GetDataNumVar(character);
        if (dataNumVar != null) dataNumVar.NumVariable -= m_WeaknessNum;
        Debug.Log("NatureElementEffectObject WeaknessEffect");
    }

    private DataNumericalVariable GetDataNumVar(ObjectT objectT)
    {
        ObjectStat objectStat = GetObjectStat(objectT);
        if (objectStat == null) return null;

        return VariableFinder.GetVariableContainNameFromList(objectStat.StatsData.DataNumVars, m_NameDataFlyweight.Name);
    }

    private ObjectStat GetObjectStat(ObjectT objectT)
    {
        if (objectT == null)
        {
            Debug.LogError("objectT is null!");
            return null;
        }

        if (objectT is IObjectStatProvider provider)
        {
            return provider.GetObjectStatByName(m_MonoBehaviourNameDataFlyweight.Name);
        }

        Debug.LogError($"Object {objectT.GetType().Name} does not implement IObjectStatProvider!");
        return null;
    }

    // private ObjectStat GetObjectStat(ObjectT objectT)
    // {
    //     if (objectT == null)
    //     {
    //         Debug.LogError("objectT is null!");
    //         return null;
    //     }

    //     // Get the type of ObjectT
    //     Type objectTType = objectT.GetType();

    //     // Try to get a field with the name m_MonoBehaviourName
    //     FieldInfo fieldInfo = objectTType.GetField(m_MonoBehaviourNameDataFlyweight.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     if (fieldInfo != null)
    //     {
    //         object fieldValue = fieldInfo.GetValue(objectT);
    //         if (fieldValue is ObjectStat objectStat)
    //             return objectStat;
    //     }

    //     // Try to get a property with the name m_MonoBehaviourName
    //     PropertyInfo propertyInfo = objectTType.GetProperty(m_MonoBehaviourNameDataFlyweight.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    //     if (propertyInfo != null)
    //     {
    //         object propertyValue = propertyInfo.GetValue(objectT);
    //         if (propertyValue is ObjectStat objectStat)
    //             return objectStat;
    //     }

    //     //Debug.LogError($"Variable {m_MonoBehaviourNameDataFlyweight.Name} not found in {objectT.name} or is not of type ObjectStat!");
    //     return null;
    // }
}
