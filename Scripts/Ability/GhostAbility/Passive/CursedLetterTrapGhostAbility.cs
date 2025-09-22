using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// FILL()
/// Texts
/// </summary>
[CreateAssetMenu(fileName = "CursedLetterTrapGhostAbility", menuName = "Abilities/Ghost/CursedLetterTrapGhostAbility")]
public class CursedLetterTrapGhostAbility : PassiveGhostAbility
{
    [SerializeField] private WordSystem m_WordSystem;
    private List<char> m_Letters = new();
    [SerializeField] protected StabilityData m_StabilityData;
    [SerializeField] private BannedLetterKey m_BannedLetterKey = new();
    [SerializeField] private Color m_TextColor = Color.red;//6
    private string m_PrevWord;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_Letters.Clear();
        m_PrevWord = string.Empty;

        //get letter from text in PassiveAbilityData
        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            m_Letters.Add(m_Words[m_Index][i]);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_Letters, 0.8f, m_TextColor);
        GhostCombatSystem.Instance.CombatUI.SetRandomPositionBadScrambleTexts(m_Letters.Count);
        // GhostCombatSystem.Instance.CombatUI.TextCenterContAnimation("Show", true);
        // GhostCombatSystem.Instance.CombatUI.TextCenterContAnimation("Swing", true);
    }

    // public override void OnTyping(string word)
    // {
    //     if (!GhostCombatSystem.Instance.CombatUI) return;

    //     if (WordChecking.CheckContainLetter(m_Letters, word))
    //     {
    //         GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("DrippingBlackInk", true);
    //     }
    //     else
    //     {
    //         GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("DrippingBlackInk", false);
    //     }
    // }

    public override void OnTyping(string word)
    {
        //Debug.Log("OnCompleteWord ability : " + word);
        if (string.IsNullOrEmpty(word) || word.Length <= 0) return;
        //init when not used backspace
        if (m_PrevWord.Length < word.Length)
        {
            if (WordChecking.CheckContainLetter(m_Letters, word[^1].ToString()))
                DoPunishment();
        }

        m_PrevWord = word;
    }

    public override void OnSuccessWord(string word)
    {
        m_PrevWord = string.Empty;
    }

    public override void DoPunishment()
    {
        int random = UnityEngine.Random.Range(0, 4);

        if (random == 0)
        {
            GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectStability.TakeDamage(m_StabilityData);
        }
        else
        {
            BannedLetterKey newKey = new()
            {
                Letter = m_WordSystem.GetRandomAlphabet(),
                Time = m_BannedLetterKey.Time // reuse timing
            };
            KeyboardManager.Instance.AddBannedLetterKeys(newKey);
        }
    }

    public override void ExitAbility()
    {
        m_Letters.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_Letters, 0);
        // GhostCombatSystem.Instance.CombatUI.TextCenterContAnimation("Show", false);
        // GhostCombatSystem.Instance.CombatUI.TextCenterContAnimation("Swing", false);
    }
}
