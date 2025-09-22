// ----------------
// Object Stat 
// Num - Magic, Mana
// Bool - CanMagic
// ----------------
public interface IMagic : IStat
{
    void Magic(string name = "normal", float cost = 0, ExecuteActionCommandData data = null);
    void Heal(MagicData magicData);
    void TakeDamage(MagicData magicData);
}

[System.Serializable]
public class MagicData : StatsMicrobarData { }
// [System.Serializable]
// public class TriggerMagicData : TriggerStatsData { }