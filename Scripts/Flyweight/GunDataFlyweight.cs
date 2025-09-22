using UnityEngine;

[CreateAssetMenu(fileName = "GunDataFlyweight", menuName = "Flyweight/Gun/GunDataFlyweight", order = 1)]
public class GunDataFlyweight : ScriptableObject
{
    public NameDataFlyweight GunNameData;
    public float MuzzleVelocity;
    public int MagazineCapacity;
    //public BulletDataFlyweight[] BulletDataFlyweightAccepted;
}
