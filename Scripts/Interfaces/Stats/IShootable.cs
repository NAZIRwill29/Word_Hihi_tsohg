// ----------------
// Object Stat 
// Num - Shoot, ShootVelocity, Bullet
// Bool - CanShoot
// ----------------
public interface IShootable : IStat
{
    void Shoot(string abilityName = "normal", float cost = 0, ExecuteActionCommandData data = null);
}

[System.Serializable]
public class ShootData : StatsData { }
// [System.Serializable]
// public class TriggerShootData : TriggerStatsData { }
