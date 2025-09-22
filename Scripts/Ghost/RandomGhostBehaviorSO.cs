using UnityEngine;

//UNUSED()
[CreateAssetMenu(fileName = "NewRandomGhostBehavior", menuName = "Ghost System/Random Ghost Behavior")]
public class RandomGhostBehaviorSO : ScriptableObject
{
    public string ghostName;
    [TextArea(2, 8)]
    public string loreSnippet;
    [TextArea(2, 8)]
    public string personality;
    [TextArea(2, 8)]
    public string behavior;
    [TextArea(2, 6)]
    public string inGameBehaviorDescription;
    public string specialEffect;
}
