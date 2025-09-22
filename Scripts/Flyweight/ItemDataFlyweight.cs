using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Flyweight/ItemData", order = 1)]
public class ItemDataFlyweight : ScriptableObject
{
    public NameDataFlyweight NameData;
    public Sprite Icon;
    public string Description;
    //for rank
    //public NameDataFlyweight RankNameData;    
    public string NegativeText = "Drop";
    public string PositiveText = "Take";
}
