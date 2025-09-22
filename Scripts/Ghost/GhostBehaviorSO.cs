using UnityEngine;

//UNUSED()
[CreateAssetMenu(fileName = "NewGhostBehavior", menuName = "Ghost System/Ghost Behavior")]
public class GhostBehaviorSO : ScriptableObject
{
    public string GhostName;
    [TextArea(2, 8)]
    public string LoreSnippet;
    [TextArea(2, 6)]
    public string Personality;
    [TextArea(2, 6)]
    public string Behavior;
    [TextArea(2, 6)]
    public string InGameBehaviorDescription;
    public string SpecialEffect;
}
