// ----------------
// Object Stat 
// Num - Stability, Invincible
// Bool - Heal, Alive
// ----------------

public interface IStability : IStat
{
    void Heal(StabilityData stabilityData, bool isSound = true);
    void TakeDamage(StabilityData stabilityData, bool isSound = true);
}

[System.Serializable]
public class StabilityData : StatsMicrobarData
{
}
