using System.Collections.Generic;
using UnityEngine;
//UNUSED()
[System.Serializable]
public class RandomGhost
{
    public string ghostName;
    public Sprite appearance;
    [TextArea(3, 10)] public string storyDescription;
    public RandomGhostBehaviorSO behaviorData; // New field to hold behavior data
    public List<string> wordPool;
    public string weaknessWord;

    [Header("Outcome Descriptions")]
    [TextArea(2, 6)] public string onSuccess;
    [TextArea(2, 6)] public string onFail;
    [TextArea(2, 6)] public string onFlee;

    public int ExpRewardNum;

    [Header("Struggle Mode")]
    public List<string> struggleMode;
}
