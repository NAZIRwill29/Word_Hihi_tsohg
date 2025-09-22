// ----------------
// Object Stat 
// Num - Hear
// Bool - CanHear
// ----------------
public interface IHear : IStat
{
    void HearChange();
}

[System.Serializable]
public class HearData : StatsData { }
// [System.Serializable]
// public class TriggerHearData : TriggerStatsData { }