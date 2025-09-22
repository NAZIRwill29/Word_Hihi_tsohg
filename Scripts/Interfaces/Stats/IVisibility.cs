// ----------------
// Object Stat 
// Num - Visibility
// Bool - CanBeSeen, CanBeHear
// ----------------
public interface IVisibility : IStat
{
    void VisibilityChange(bool isCanBeSeen, bool isCanBeHear);
}

[System.Serializable]
public class VisibilityData : StatsData { }

// [System.Serializable]
// public class TriggerVisibilityData : TriggerStatsData { }
