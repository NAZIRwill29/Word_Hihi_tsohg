using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Flyweight/Equipment/ProjectileData", order = 1)]
public class ProjectileDataFlyweight : ScriptableObject
{
    public float ForceMultiFactor = 1;
    public float LaunchTimeCooldown = 4f;
    public float Distance = 20;
}