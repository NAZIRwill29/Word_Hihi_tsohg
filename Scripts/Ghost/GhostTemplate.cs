using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class GhostTemplate : ScriptableObject
{
    [Header("Basic Info")]
    public NameDataFlyweight GhostNameData;
    [Tooltip("to be shown in ghost profile / journal")]
    public Sprite Appearance;
    [ExpandableTextArea] public string AppearanceDescription;
    [ExpandableTextArea] public string StoryDescription;
    [ExpandableTextArea] public string PromptDescription;
    public Sprite PromptBoxSprite;
    public Sprite PromptUISprite;
    public Sprite GhostCharSprite;
    public List<SpriteWithName> SpriteEmotions;

    [Header("Behavior"), ExpandableTextArea]
    public string Behavior;
    [ExpandableTextArea] public string Personality;

    [Header("Ability")]
    public List<PassiveGhostAbility> PassiveGhostAbilities;
    public List<ActiveGhostAbility> ActiveGhostAbilities;
    public List<PassiveGhostAbility> AdvancePassiveGhostAbilities;
    public List<PassiveGhostAbility> AdvanceActiveGhostAbilities;
    public string SpecialEffect;

    [Header("Mana")]
    public float RecoveryManaCalm = 1;
    public float RecoveryManaAnger = 1;

    // [Header("Movement")]
    // public float GhostMoveTime;
    // public float DistancePercentChange;
    // [Range(0, 15)] public float DistancePercentChangePlusPush;
    // [Range(-15, 0)] public float DistancePercentChangePlusPull;

    [Header("Weakness Words")]
    public List<string> WeaknessWords;
    public List<string> StoryBasedTriggerWords;
    public int ActiveWordCanTriggerWeaknessWordNum { get; set; }

    [Header("Outcome Descriptions")]
    [ExpandableTextArea] public string OnDeath;
    [ExpandableTextArea] public string OnBanished;
    [ExpandableTextArea] public string OnFailHealth;
    [ExpandableTextArea] public string OnFailStability;
    [ExpandableTextArea] public string OnFlee;
    public List<GameOverData> GameOverDatas;

    public int ExpRewardNum;
    public int EscapeNum = 3;

    public virtual void Initialize(WordPoolManager wordPoolManager)
    {
        ActiveWordCanTriggerWeaknessWordNum = Random.Range(0, WeaknessWords.Count);
        EscapeNum = 3;
    }
}
