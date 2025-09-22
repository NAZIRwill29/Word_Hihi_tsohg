// ----------------
// Object Stat 
// Num - Sight
// Bool - CanSee
// ----------------
public interface ISight : IStat
{
    void Sight();
}

[System.Serializable]
public class SightData : StatsData { }
// [System.Serializable]
// public class TriggerSightData : TriggerStatsData { }