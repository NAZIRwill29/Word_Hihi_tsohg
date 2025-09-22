using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfernoCloakGhostAbility", menuName = "Abilities/Ghost/InfernoCloakGhostAbility")]
public class InfernoCloakGhostAbility : ActiveGhostAbility
{
    private int m_FireLevel, m_FireLevelEnd;
    [SerializeField] private int m_FireLevelEntry = 6;
    private string m_PrevWord;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);

        m_FireLevel = m_FireLevelEntry;
        m_FireLevelEnd = GhostCombatSystem.Instance.CombatUI.StruggleModeTextBlockMax;

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_Words);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextBlockAnimation("Fire", true);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeTextBlocks(m_FireLevel, true);
    }

    public override void OnSuccessWord(string word)
    {
        if (WordChecking.CheckIsWordExistInList(m_Words, word) && m_PrevWord != word)
        {
            NextChallenge();
            m_PrevWord = word;
        }
        else
            PrevChallenge();
    }

    public override void OnFailedWord(string word)
    {
        PrevChallenge();
    }

    protected override void NextChallenge()
    {
        //increase fire
        Debug.Log("NextChallenge");
        m_FireLevel--;
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeTextBlock(m_FireLevel, false);
        if (m_FireLevel <= 0)
            DoReward();
    }

    private void PrevChallenge()
    {
        //decrease fire
        Debug.Log("PrevChallenge");
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeTextBlock(m_FireLevel, true);
        m_FireLevel++;
        if (m_FireLevel >= m_FireLevelEnd)
            DoPunishment();
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_FireLevel = 0;
        m_Words.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeTexts(m_Words);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextBlockAnimation("Fire", false);
    }
}
