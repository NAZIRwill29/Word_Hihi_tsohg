// ----------------
// Object Stat 
// Num - Melee
// Bool - CanMelee
// ----------------
public interface IMeleable : IStat
{
    void Melee(string name = "normal", float cost = 0);
}

[System.Serializable]
public class MeleeData : StatsData { }
// [System.Serializable]
// public class TriggerMeleeData : TriggerStatsData { }
