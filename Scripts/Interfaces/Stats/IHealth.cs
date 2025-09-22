// ----------------
// Object Stat 
// Num - Health, Invincible
// Bool - Alive, Heal, CanInvincible
// ----------------
using System.Linq;

public interface IHealth : IStat
{
    //void Heal(float amount);
    void Heal(HealthData healthData);
    void TakeDamage(HealthData healthData);
}

[System.Serializable]
public class StatsMicrobarData : StatsData
{
    public MicrobarAnimType MicrobarAnimType = MicrobarAnimType.simple;
}

[System.Serializable]
public class HealthData : StatsMicrobarData
{
}

// [System.Serializable]
// public class TriggerHealthData : TriggerStatsData
// {
//     public MicrobarAnimType MicrobarAnimType = MicrobarAnimType.simple;
// }