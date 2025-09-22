using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostCombatDataFlyweight", menuName = "Flyweight/GhostCombatDataFlyweight", order = 1)]
public class GhostCombatDataFlyweight : ScriptableObject
{
    [Header("Change Rule Time")]
    public float ChangeRuleTime;

    // [Header("Change Ability Time")]
    // public float ChangeAbilityTime;

    [Header("Change Ability Rest Time")]
    public float ChangeAbilityRestTime;

    // [Header("Active Struggle Mode Time")]
    // public float ActiveStruggleModeTime;

    [Header("Letter Placement")]
    public List<LetterPlacementSO> LetterPlacementSOs;

    [Header("Letter Rule")]
    public List<LetterRuleSO> LetterRuleSOs;
}
