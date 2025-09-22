using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListPoisonData
{
    public List<PoisonData> PoisonData = new();

    public void Reset()
    {
        PoisonData.Clear();
    }
}

[Serializable]
public class PoisonData : NameVariable
{
    public float AddNum = 0;
    public bool IsNumber = false;

    // Reset method to clear data before reusing
    public void Reset()
    {
        Name = string.Empty;
        AddNum = 0;
        IsNumber = false;
    }
}

[CreateAssetMenu(fileName = "ObjectPoisonable", menuName = "ObjectStat/ObjectPoisonable", order = 1)]
public class ObjectPoisonable : ScriptableObject
{
    public void Cooldown(ObjectT objectT, ObjectStat objectStat)
    {
        if (objectStat.StatsData == null || !objectT.IsAlive) return;

        if (objectStat.StatsData.DataNumVars != null)
        {
            foreach (var item in objectStat.StatsData.DataNumVars)
            {
                if (item.IsPoison)
                {
                    if (!objectT.IsAlive)
                    {
                        item.PoisonCooldown = item.PoisonTimeCoolDown;
                        if (!objectStat.IsInZoneTrigger)
                            item.PoisonDuration = 0;
                    }
                    else
                    {
                        item.PoisonCooldown -= Time.deltaTime;
                        if (!objectStat.IsInZoneTrigger)
                            item.PoisonDuration -= Time.deltaTime;
                    }
                }
            }
        }

        if (objectStat.StatsData.DataBoolVars != null)
        {
            foreach (var item in objectStat.StatsData.DataBoolVars)
            {
                if (item.IsPoison && !objectStat.IsInZoneTrigger)
                {
                    if (!objectT.IsAlive)
                        item.PoisonDuration = 0;
                    else
                        item.PoisonDuration -= Time.deltaTime;
                }
            }
        }
    }

    public void Poison(ObjectStat objectStat)
    {
        bool poison = false;

        // Get a ListPoisonData object from the pool
        ListPoisonData listPoisonData = GenericObjectPool<ListPoisonData>.Get();
        listPoisonData.Reset();

        // Get a pooled List<PoisonData> instead of creating a new one
        List<PoisonData> poisonDataList = GenericObjectPool<List<PoisonData>>.Get();
        poisonDataList.Clear();

        foreach (var item in objectStat.StatsData.DataNumVars)
        {
            if (item.IsPoison && item.PoisonDuration >= 0 && item.PoisonCooldown <= 0)
            {
                item.NumVariable += item.PoisonAmount;
                item.PoisonCooldown = item.PoisonTimeCoolDown;
                poison = true;

                // Get a PoisonData object from the generic pool instead of allocating a new one
                PoisonData poisonData = GenericObjectPool<PoisonData>.Get();
                poisonData.Name = item.Name;
                poisonData.AddNum = item.PoisonAmount;
                poisonData.IsNumber = true;

                poisonDataList.Add(poisonData);
            }
        }

        listPoisonData.PoisonData = poisonDataList;

        if (poison)
        {
            objectStat.PoisonHappened(listPoisonData);
        }

        // Return PoisonData objects to the pool after processing
        foreach (var poisonData in poisonDataList)
        {
            GenericObjectPool<PoisonData>.Return(poisonData, obj => obj.Reset());
        }

        // Return the List<PoisonData> to the pool instead of garbage collecting it
        GenericObjectPool<List<PoisonData>>.Return(poisonDataList, obj => obj.Clear());

        // Return ListPoisonData to the pool
        GenericObjectPool<ListPoisonData>.Return(listPoisonData, obj => obj.Reset());
    }

    // public void Poison(ObjectStat objectStat)
    // {
    //     bool poison = false;

    //     // Get a fresh ListPoisonData object from the generic pool
    //     ListPoisonData listPoisonData = GenericObjectPool<ListPoisonData>.Get();
    //     listPoisonData.Reset();

    //     foreach (var item in objectStat.StatsData.DataNumVars)
    //     {
    //         if (item.IsPoison && item.PoisonDuration >= 0 && item.PoisonCooldown <= 0)
    //         {
    //             item.NumVariable += item.PoisonAmount;
    //             item.PoisonCooldown = item.PoisonTimeCoolDown;
    //             poison = true;

    //             // Get a PoisonData object from the generic pool
    //             PoisonData poisonData = GenericObjectPool<PoisonData>.Get();
    //             poisonData.Name = item.Name;
    //             poisonData.AddNum = item.PoisonAmount;
    //             poisonData.IsNumber = true;

    //             listPoisonData.PoisonData.Add(poisonData);
    //         }
    //     }

    //     if (poison)
    //     {
    //         objectStat.PoisonHappened(listPoisonData);
    //     }

    //     // Return PoisonData objects to the pool after processing
    //     foreach (var poisonData in listPoisonData.PoisonData)
    //     {
    //         GenericObjectPool<PoisonData>.Return(poisonData, obj => obj.Reset());
    //     }

    //     // Return ListPoisonData to the pool
    //     GenericObjectPool<ListPoisonData>.Return(listPoisonData, obj => obj.Reset());
    // }

    // public void Poison(ObjectStat objectStat)
    // {
    //     bool poison = false;
    //     ListPoisonData listPoisonData = new()
    //     {
    //         PoisonData = new List<PoisonData>()
    //     };
    //     foreach (var item in objectStat.StatsData.DataNumVars)
    //     {
    //         if (item.IsPoison && item.PoisonDuration >= 0 && item.PoisonCooldown <= 0)
    //         {
    //             item.NumVariable += item.PoisonAmount;
    //             //Debug.Log("PChangeNumVariable " + item.Name + " " + item.NumVariable + " poison " + item.PoisonAmount);
    //             item.PoisonCooldown = item.PoisonTimeCoolDown;
    //             poison = true;

    //             listPoisonData.PoisonData.Add(new()
    //             {
    //                 Name = item.Name,
    //                 AddNum = item.PoisonAmount,
    //                 IsNumber = true,
    //             });
    //         }
    //     }
    //     if (poison)
    //         objectStat.PoisonHappened(listPoisonData);
    // }
}
