using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListRecoveryData
{
    public List<RecoveryData> RecoveryData;
}

[Serializable]
public class RecoveryData : INameable
{
    public string Name { get; set; }
    public float AddNum = 0;
    public bool IsNumber = false;
}

[CreateAssetMenu(fileName = "ObjectRecoverable", menuName = "ObjectStat/ObjectRecoverable", order = 1)]
public class ObjectRecoverable : ScriptableObject
{
    [SerializeField] private float m_RecoveryTimeCooldown = 0.5f;
    [SerializeField, Range(0, 0.1f)] private float m_DelayRecoveryTime = 0.05f;

    public void Initialize(ObjectStat objectStat)
    {
        foreach (var item in objectStat.StatsData.DataNumVars)
        {
            item.DelayRecoveryCooldown = m_DelayRecoveryTime;
        }
        foreach (var item in objectStat.StatsData.DataBoolVars)
        {
            item.DelayRecoveryCooldown = m_DelayRecoveryTime;
        }
    }

    public void Recovery(ObjectT objectT, ObjectStat objectStat)
    {
        //RCOVERY(2)
        if (!objectT.IsAlive)
            return;
        if (objectStat.IsInZoneTrigger)
            return;
        if (objectStat.RecoveryCooldown > 0)
        {
            objectStat.RecoveryCooldown -= Time.deltaTime;
            return;
        }

        //Debug.Log("Recovery");

        // Recover data
        objectStat.StatsData.DataBoolVars = RecoverBoolData(objectT, objectStat, objectStat.StatsData.DataBoolVars);
        objectStat.StatsData.DataNumVars = RecoverNumericalData(objectT, objectStat, objectStat.StatsData.DataNumVars);

        // Reset recovery cooldown
        objectStat.RecoveryCooldown = m_RecoveryTimeCooldown;
    }

    // Recovery for boolean data
    private List<T> RecoverBoolData<T>(ObjectT objectT, ObjectStat objectStat, List<T> data) where T : DataBoolVariable
    {
        if (data == null) return data;

        bool recoveryOccurred = false;

        ListRecoveryData listRecoveryData = new()
        {
            RecoveryData = new List<RecoveryData>()
        };

        foreach (var item in data)
        {
            if (item == null) continue;
            if (!VariableChanger.IsBoolNull(item.IsCanRecovery)) continue;
            if (VariableChanger.IsBoolNull(item.IsCan)) continue;
            if (item.IsPoison) continue;
            //yes no con, yes yes ->, no yes ->
            if (!objectT.InBattle)
            {
                if (!item.IsCanRecoveryInNormal)
                {
                    item.DelayRecoveryCooldown = m_DelayRecoveryTime;
                    continue;
                }
            }
            else
            {
                if (!item.IsCanRecoveryInBattle)
                {
                    item.DelayRecoveryCooldown = m_DelayRecoveryTime;
                    continue;
                }
            }

            if (!VariableChanger.IsBoolNull(item.IsCan))
            {
                item.DelayRecoveryCooldown -= Time.deltaTime;
                if (item.DelayRecoveryCooldown > 0) continue;

                //Debug.Log("RecoverBoolData");
                item.IsCan = BoolNull.IsTrue;
                recoveryOccurred = true;
                listRecoveryData.RecoveryData.Add(new()
                {
                    Name = item.Name,
                    IsNumber = false,
                });
            }
        }

        if (recoveryOccurred)
        {
            objectStat.RecoveryHappened(listRecoveryData);
        }

        return data;
    }

    // Recovery for numerical data
    private List<T> RecoverNumericalData<T>(ObjectT objectT, ObjectStat objectStat, List<T> data) where T : DataNumericalVariable
    {
        //RCOVERY(3)
        if (data == null) return data;

        bool recoveryOccurred = false;

        ListRecoveryData listRecoveryData = new()
        {
            RecoveryData = new List<RecoveryData>()
        };

        foreach (var item in data)
        {
            if (item == null) continue;
            if (!VariableChanger.IsBoolNull(item.IsCanRecovery)) continue;
            if (item.IsPoison) continue;
            //Debug.Log("RecoverNumericalData 0");
            //yes no con, yes yes ->, no yes ->
            if (!objectT.InBattle)
            {
                if (!item.IsCanRecoveryInNormal)
                {
                    item.DelayRecoveryCooldown = m_DelayRecoveryTime;
                    continue;
                }
            }
            else
            {
                if (!item.IsCanRecoveryInBattle)
                {
                    item.DelayRecoveryCooldown = m_DelayRecoveryTime;
                    continue;
                }
            }
            item.DelayRecoveryCooldown -= Time.deltaTime;

            //Debug.Log("RecoverNumericalData 1");
            if (item.Cooldown <= 0)
            {
                float numTarget = item.IsRecoverMax ? item.NumVariableMax : item.NumVariableOri;
                //Debug.Log("RecoverNumericalData 2 " + numTarget);

                if (item.NumVariable == numTarget) continue;
                if (item.DelayRecoveryCooldown > 0) continue;

                //Debug.Log("RecoverNumericalData 3");
                float numDiff = Mathf.Abs(item.NumVariable - numTarget);
                float adjustment = item.RecoveryNum * Mathf.Sign(numTarget - item.NumVariable);
                //float adjustment = Mathf.Max(1, item.NumVariableMax / 100) * Mathf.Sign(numTarget - item.NumVariable);

                item.PoisonAmount = 0;

                float addNUm = 0;
                if (Mathf.Abs(item.NumVariable - numTarget) > 1)
                    addNUm = adjustment;
                else
                    addNUm = numTarget - item.NumVariable;

                // Ensure we don't overshoot
                float newValue = item.NumVariable + addNUm;
                item.NumVariable = Mathf.Clamp(
                    newValue,
                    Mathf.Min(item.NumVariable, numTarget),
                    Mathf.Max(item.NumVariable, numTarget)
                );

                listRecoveryData.RecoveryData.Add(new()
                {
                    Name = item.Name,
                    IsNumber = true,
                    AddNum = addNUm,
                });

                //Debug.Log("RecoverNumericalData " + item.NumVariable);
                recoveryOccurred = true;
            }
        }

        if (recoveryOccurred)
        {
            objectStat.RecoveryHappened(listRecoveryData);
        }

        return data;
    }
}
