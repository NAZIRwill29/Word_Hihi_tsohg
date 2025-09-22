using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HairBindTrapGhostAbility", menuName = "Abilities/Ghost/HairBindTrapGhostAbility")]
public class HairBindTrapGhostAbility : ActiveGhostAbility
{
    [SerializeField] private int m_LetterNum;
    [SerializeField] private Sprite m_BindSprite;

    private string m_TypingWord = string.Empty;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);

        // Reset typing and challenge index
        m_Index = 0;
        SetChallenge();

        // Start UI flicker and bind images
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", true);
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigTextBindImages(m_BindSprite);
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
            // Mistyped: reset current challenge
            SetChallenge();
        }
    }

    protected override void NextChallenge()
    {
        int lastIndex = Mathf.Max(0, m_TypingWord.Length - 1);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImage(lastIndex, false);
    }

    private void SetChallenge()
    {
        m_TypingWord = string.Empty;
        m_Words[m_Index] = string.Empty;

        int count = Math.Min(GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextContMax, m_LetterNum);
        for (int i = 0; i < count; i++)
        {
            m_Words[m_Index] += GhostCombatSystem.Instance.WordSystem.GetRandomAlphabet();
        }

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImages(true);

        if (m_CombatTypingSystem != null)
            m_CombatTypingSystem.StartSystem(true);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();

        // Reset state
        m_Index = 0;
        m_TypingWord = string.Empty;
        m_Words[m_Index] = string.Empty;

        // Clear UI
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeBigText(string.Empty);
        GhostCombatSystem.Instance.CombatUI.ShowStruggleModeBigTextBindImages(false);
        GhostCombatSystem.Instance.CombatUI.StruggleModeBigTextCircleAnimation("Flicker", false);
    }
}
