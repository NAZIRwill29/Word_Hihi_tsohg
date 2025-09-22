using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TiedLetterBreakGhostAbility", menuName = "Abilities/Ghost/TiedLetterBreakGhostAbility")]
public class TiedLetterBreakGhostAbility : ActiveGhostAbility
{
    [SerializeField] private Sprite m_BindSprite;
    // [SerializeField] private int m_AdvanceForceMult = 2;
    private string m_TypingWord = string.Empty;
    [SerializeField] private MagicData healMagicData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);

        // Reset typing and challenge index
        m_TypingWord = string.Empty;

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImages(true);

        // Start UI flicker and bind images
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigTextBindImages(m_BindSprite);

        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            GhostCombatSystem.Instance.CombatUI.SetStruggleModeBigTextVisibility(i, i % 2 == 0 ? 1 : 0);
        }
    }

    public override void OnTyping(string word)
    {
        // null detection && Backspace detection: if new word shorter than previous
        if (string.IsNullOrEmpty(word) || m_TypingWord.Length >= word.Length)
        {
            m_TypingWord = word;
            return;
        }

        m_TypingWord = word;
        int pos = m_TypingWord.Length - 1;

        // Correct letter typed?
        if (pos < m_Words[m_Index].Length && m_TypingWord[pos] == m_Words[m_Index][pos])
        {
            if (m_TypingWord.Length == m_Words[m_Index].Length)
            {
                DoReward();
            }
            else
            {
                NextChallenge();
            }
        }
        else
        {
            // Mistyped: ghost mana increase
            GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectMagic.Heal(healMagicData);
            // Mistyped: ghost advance
            // float basePull = GhostCombatSystem.Instance.GhostTemplate.DistancePercentChangePlusPull;
            // float totalPull = basePull * m_AdvanceForceMult;
            // GhostCombatSystem.Instance.CombatSystem.CombatDistanceSystem.AdvanceNow(totalPull);
        }
    }

    protected override void NextChallenge()
    {
        int lastIndex = Mathf.Max(0, m_TypingWord.Length - 1);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImage(lastIndex, false);
        GhostCombatSystem.Instance.CombatUI.SetStruggleModeBigTextVisibility(lastIndex, 1);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();

        // Reset state
        m_TypingWord = string.Empty;
        m_Words.Clear();

        // Clear UI
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText(string.Empty);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImages(false);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", false);
    }
}
