// ----------------
// Object Stat 
// Num - Stamina
// Bool - CanMove
// ----------------
public interface IStamina : IStat
{
    void StaminaChange();
}

[System.Serializable]
public class StaminaData : StatsData { }
// [System.Serializable]
// public class TriggerStaminaData : TriggerStatsData { }
