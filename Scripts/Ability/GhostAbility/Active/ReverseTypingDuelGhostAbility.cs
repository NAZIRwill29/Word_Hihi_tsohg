using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ReverseTypingDuelGhostAbility", menuName = "Abilities/Ghost/ReverseTypingDuelGhostAbility")]
public class ReverseTypingDuelGhostAbility : ActiveGhostAbility
{

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        for (int i = 0; i < m_Words.Count; i++)
        {
            m_Words[i] = WordEffectUtils.ReverseWord(m_Words[i]);
        }
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ChangeIsCheckWordNotExist(false);
    }

    public override void OnCompleteWord(string word)
    {
        //Debug.Log("OnCompleteWord ability : " + word);
        if (m_Words[m_Index] == word)
            NextChallenge();
    }

    protected override void NextChallenge()
    {
        base.NextChallenge();

        if (m_Index < m_WordCount)
        {
            m_TextCooldown = m_TextTime;
            GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
            GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
        }
        else
            DoReward();
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(string.Empty);
        GhostCombatSystem.Instance.CombatUI.ChangeIsCheckWordNotExist(true);
    }
}