using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StoryGhostTemplate", menuName = "Ghost System/Story Ghost Template")]
public class StoryGhostTemplate : GhostTemplate
{
    [Header("Clues")]
    public List<string> ClueSources;

    public List<string> Rewards;

    public override void Initialize(WordPoolManager wordPoolManager)
    {
        base.Initialize(wordPoolManager);
    }
}
