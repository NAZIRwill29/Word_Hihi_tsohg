using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RandomGhostTemplate", menuName = "Ghost System/Random Ghost Template")]
public class RandomGhostTemplate : GhostTemplate
{
    public List<NameDataFlyweight> WordCategoryNameDatas;

    [ReadOnly] public List<string> WordCategories;

    [Header("All Story Based Trigger Word"), ReadOnly]

    public List<string> AllStoryBasedTriggerWord;
    //
    public override void Initialize(WordPoolManager wordPoolManager)
    {
        foreach (var item in WordCategoryNameDatas)
        {
            WordCategories.Add(item.Name);
        }

        ChangeWordPool(wordPoolManager);
        base.Initialize(wordPoolManager);
    }

    public void ChangeWordPool(WordPoolManager wordPoolManager)
    {
        WeaknessWords = wordPoolManager.GetRandomWords(WordCategories, WordCategories.Count);
    }
}
