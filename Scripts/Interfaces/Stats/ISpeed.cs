// ----------------
// Object Stat 
// Num - Speed
// Bool - CanWalk
// ----------------
public interface ISpeed : IStat
{
    void SpeedChangeByPass(float addNum);
    public void CanWalkChange(bool isTrue);
}

[System.Serializable]
public class SpeedData : StatsData { }
// [System.Serializable]
// public class TriggerSpeedData : TriggerStatsData { }