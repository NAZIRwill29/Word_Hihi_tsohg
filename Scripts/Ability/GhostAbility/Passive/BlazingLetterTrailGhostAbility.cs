using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlazingLetterTrailGhostAbility", menuName = "Abilities/Ghost/BlazingLetterTrailGhostAbility")]
public class BlazingLetterTrailGhostAbility : PassiveGhostAbility
{
    private List<char> m_Letters = new();
    public int NumberLetterToHide;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_Letters.Clear();
        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            m_Letters.Add(m_Words[m_Index][i]);
        }
    }

    public override void OnTyping(string word)
    {
        if (!GhostCombatSystem.Instance.CombatUI) return;

        if (WordChecking.CheckContainLetter(m_Letters, word))
        {
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("FieryTrails", true);
        }
        else
        {
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("FieryTrails", false);
        }
    }

    public override void OnSuccessWord(string word)
    {
        //Debug.Log("OnCompleteWord ability : " + word);
        if (WordChecking.CheckContainLetter(m_Letters, word))
            DoPunishment();
    }
    public override void DoPunishment()
    {
        if (GhostCombatSystem.Instance != null &&
            GhostCombatSystem.Instance.CombatSystem != null &&
            GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem != null)
        {
            GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.HideExorcismLetter(NumberLetterToHide);
        }
        else
        {
            Debug.LogWarning("Combat system or exorcism letter system not set up properly.");
        }
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        if (GhostCombatSystem.Instance.CombatUI)
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("FieryTrails", false);
    }
}
