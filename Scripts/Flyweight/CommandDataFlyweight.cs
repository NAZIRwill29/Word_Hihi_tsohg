using UnityEngine;

[CreateAssetMenu(fileName = "CommandData", menuName = "Flyweight/CommandData/CommandData", order = 1)]
public class CommandDataFlyweight : ScriptableState
{
    public NameDataFlyweight IdNameData;
    public NameDataFlyweight CmdNameData;
    public string IdName { get => IdNameData.Name; }
    public string CmdName { get => CmdNameData.Name; }
}
