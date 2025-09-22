using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListString", menuName = "Flyweight/ListString", order = 1)]
public class ListStringDataFlyweight : ScriptableObject
{
    public List<string> Strings;
}
