using UnityEngine;

// ----------------
// Object Stat 
// Num - Defense, ShootDefense, MeleeDefense, MagicDefense
// Bool - CanDefense
// ----------------
public interface IDefense : IStat
{
    void Defense(string name = "normal", float cost = 0);
}

// [System.Serializable]
// public class DefenseUpdateData : StatsData { }

[System.Serializable]
public class DefenseData : StatsData { }
// [System.Serializable]
// public class TriggerDefenseData : TriggerStatsData { }
