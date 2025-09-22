using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirestormModeGhostAbility", menuName = "Abilities/Ghost/FirestormModeGhostAbility")]
public class FirestormModeGhostAbility : ActiveGhostAbility
{
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private WordCheck m_WordCheck;
    private List<char> m_CurseLetters = new();
    private List<char> m_ExorcismLetters = new();
    [SerializeField] private int m_BadLetterCount = 15;
    [SerializeField] private HealthData m_BurnHealthData;
    [SerializeField] private float m_MinSpeed = 50, m_MaxSpeed = 100;
    [SerializeField] private float m_FlickerTime = 5;
    private string m_PrevWord;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_PrevWord = string.Empty;
        ChangeCurseLetter();
        ChangeExorcismLetter(m_Words[m_Index]);

        //GhostCombatSystem.Instance.CombatUI.InitializeAllMovingTexts(m_ExorcismLetters.Count, m_CurseLetters.Count, m_MinSpeed, m_MaxSpeed);
        GhostCombatSystem.Instance.CombatUI.InitializeBadMovingTexts(m_CurseLetters.Count, m_MinSpeed, m_MaxSpeed);

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText("");
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);

        for (int i = 0; i < m_Words[m_Index].Length; i++)
        {
            GhostCombatSystem.Instance.CombatUI.SetGoodScrambleTextFlicker(i, i + 1);
        }
        GhostCombatSystem.Instance.CombatUI.SetSrambleTextFlickerTime(m_FlickerTime);
    }

    public override void OnTyping(string word)
    {
        if (string.IsNullOrEmpty(word) || word.Length <= 0) return;
        //init when not used backspace
        if (m_PrevWord.Length < word.Length)
        {
            string lastLetter = word[^1].ToString();
            if (WordChecking.CheckContainLetter(m_CurseLetters, lastLetter) && !WordChecking.CheckContainLetter(m_ExorcismLetters, lastLetter))
                BurnPlayer();
        }

        m_PrevWord = word;
    }

    public override void OnSuccessWord(string word)
    {
        if (m_Words[m_Index] == word)
            DoReward();
        else
            BurnPlayer();
        m_PrevWord = string.Empty;
    }

    private void BurnPlayer()
    {
        GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("FieryTrailsTrig");
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_BurnHealthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }

    private void ChangeCurseLetter()
    {
        m_CurseLetters.Clear();

        for (int i = 0; i < m_BadLetterCount; i++)
        {
            m_CurseLetters.Add(m_WordSystem.GetRandomAlphabet());
        }

        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_CurseLetters, 1);
        GhostCombatSystem.Instance.CombatUI.SetRandomPositionBadScrambleTexts(m_CurseLetters.Count);
    }

    private void ChangeExorcismLetter(string word)
    {
        m_ExorcismLetters.Clear();

        for (int i = 0; i < word.Length; i++)
        {
            m_ExorcismLetters.Add(word[i]);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeGoodScrambleTexts(m_ExorcismLetters, 1);
        GhostCombatSystem.Instance.CombatUI.SetRandomPositionGoodScrambleTexts(m_CurseLetters.Count);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_CurseLetters.Clear();
        m_ExorcismLetters.Clear();
        m_Words.Clear();

        GhostCombatSystem.Instance.CombatUI.ChangeBadScrambleTexts(m_CurseLetters, 0);
        GhostCombatSystem.Instance.CombatUI.ChangeGoodScrambleTexts(m_ExorcismLetters, 0);
        GhostCombatSystem.Instance.CombatUI.InitializeAllMovingTexts(m_ExorcismLetters.Count, m_CurseLetters.Count, m_MinSpeed, m_MaxSpeed);
        GhostCombatSystem.Instance.CombatUI.ReseFlickerUI();
    }
}
