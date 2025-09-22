// ----------------
// Object Stat 
// Num - StrengthShoot, StrengthMelee, StrengthMagic
// Bool - 
// ----------------
public interface IStrength : IStat
{
    void StrengthChange();
}

[System.Serializable]
public class StrengthData : StatsData { }
// [System.Serializable]
// public class TriggerStrengthData : TriggerStatsData { }