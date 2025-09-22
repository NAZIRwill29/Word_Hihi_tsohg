using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataFlyweight", menuName = "Flyweight/Gun/BulletDataFlyweight", order = 1)]
public class BulletDataFlyweight : ScriptableObject
{
    public NameDataFlyweight BulletNameData;
    public int BulletSize;
}
