// ----------------
// Object Stat 
// Num - Level, Exp
// Bool - CanLevelUp
// ----------------
public interface ILevel : IStat
{
    void LevelChange();
}

[System.Serializable]
public class LevelData : StatsData { }
// [System.Serializable]
// public class TriggerLevelData : TriggerStatsData { }
