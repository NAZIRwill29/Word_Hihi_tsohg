using UnityEngine;

// ----------------
// Object Stat 
// Num - Evade
// Bool - CanEvade
// ----------------
public interface IEvadeable : IStat
{
    void Evade(string name = "normal", float cost = 0);
}

[System.Serializable]
public class EvadeData : StatsData { }
// [System.Serializable]
// public class TriggerEvadeData : TriggerStatsData { }