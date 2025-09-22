using UnityEngine;

[CreateAssetMenu(fileName = "CommandPooledObjData", menuName = "Flyweight/CommandData/CommandPooledObjData", order = 1)]
public class CommandPooledObjDataFlyweight : CommandDataFlyweight
{
    public NameDataFlyweight PooledObjNameDataFlyweight;
}
