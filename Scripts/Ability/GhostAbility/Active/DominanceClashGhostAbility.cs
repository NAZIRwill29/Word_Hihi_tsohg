using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DominanceClashGhostAbility", menuName = "Abilities/Ghost/DominanceClashGhostAbility")]
public class DominanceClashGhostAbility : ActiveGhostAbility
{
    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);
    }

    public override void OnSuccessWord(string word)
    {
        Debug.Log("OnCompleteWord ability : " + word);
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
    }
}
