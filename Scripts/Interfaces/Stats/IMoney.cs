// ----------------
// Object Stat 
// Num - Money
// Bool - CanCollectMoney
// ----------------
public interface IMoney : IStat
{
    void MoneyChange();
}

[System.Serializable]
public class MoneyData : StatsData { }
// [System.Serializable]
// public class TriggerMoneyData : TriggerStatsData { }
