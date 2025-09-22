using UnityEngine;

[CreateAssetMenu(fileName = "TextFloatData", menuName = "Flyweight/TextFloatData", order = 1)]
public class TextFloatDataFlyweight : ScriptableObject
{
    public string Msg;
    public int FontSize = 20;
    public Color Color = Color.green;
    public Vector3 Motion = Vector3.up * 25;
    public float Duration = 1.5f;
}
