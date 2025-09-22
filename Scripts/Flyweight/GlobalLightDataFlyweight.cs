using UnityEngine;

[CreateAssetMenu(fileName = "GlobalLightData", menuName = "Flyweight/GlobalLightData", order = 1)]
public class GlobalLightDataFlyweight : ScriptableObject
{
    public Color Color;
    public float Intensity;
    public string[] TargetSortingLayerNames;
}
