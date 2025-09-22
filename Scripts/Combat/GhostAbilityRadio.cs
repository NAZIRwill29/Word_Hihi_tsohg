using System.Collections.Generic;
using UnityEngine;

public class GhostAbilityRadio : MonoBehaviour
{
    public CombatSystem CombatSystem;
    [SerializeField] private WordSystem m_WordSystem;

    private void OnEnable()
    {
        if (CombatSystem)
        {
            if (CombatSystem.LetterRuleSystem)
            {
                CombatSystem.LetterRuleSystem.OnSuccesLetterRule.AddListener(OnSuccesLetterRule);
                CombatSystem.LetterRuleSystem.OnFailedLetterRule.AddListener(OnFailedLetterRule);
            }
            if (CombatSystem.CombatTypingSystem)
            {
                CombatSystem.CombatTypingSystem.OnTyping.AddListener(OnTyping);
                CombatSystem.CombatTypingSystem.OnExorcismTyping.AddListener(OnExorcismTyping);
                CombatSystem.CombatTypingSystem.OnCompleteWord.AddListener(OnCompleteWord);
            }
            if (CombatSystem.CombatUI)
            {
                CombatSystem.CombatUI.OnFlickerStartBright.AddListener(OnFlickerStartBright);
            }
            if (CombatSystem.ExorcismLetterSystem)
            {
                CombatSystem.ExorcismLetterSystem.OnChangeExorcismLetter.AddListener(OnChangeExorcismLetter);
                CombatSystem.ExorcismLetterSystem.OnUseInCannotUseExorcismLetter.AddListener(OnUseInCannotUseExorcismLetter);
            }
            if (CombatSystem.ExorcismWordSystem)
            {
                CombatSystem.ExorcismWordSystem.OnExorciseWeaknessWord.AddListener(OnExorciseWeaknessWord);
                CombatSystem.ExorcismWordSystem.OnExorciseCurseWord.AddListener(OnExorciseCurseWord);
            }
        }
        if (m_WordSystem)
        {
            m_WordSystem.OnSuccessWord.AddListener(OnSuccessWord);
            m_WordSystem.OnFailedWord.AddListener(OnFailedWord);
            m_WordSystem.OnSuccessWordNotDuplicate.AddListener(OnSuccessWordNotDuplicate);
            m_WordSystem.OnFailedWordNotDuplicate.AddListener(OnFailedWordNotDuplicate);
            m_WordSystem.OnSuccessExorcismWord.AddListener(OnSuccessExorcismWord);
            m_WordSystem.OnFailedExorcismWord.AddListener(OnFailedExorcismWord);
        }
    }

    private void OnDisable()
    {
        if (CombatSystem)
        {
            if (CombatSystem.LetterRuleSystem)
            {
                CombatSystem.LetterRuleSystem.OnSuccesLetterRule.RemoveListener(OnSuccesLetterRule);
                CombatSystem.LetterRuleSystem.OnFailedLetterRule.RemoveListener(OnFailedLetterRule);
            }
            if (CombatSystem.CombatTypingSystem)
            {
                CombatSystem.CombatTypingSystem.OnTyping.RemoveListener(OnTyping);
                CombatSystem.CombatTypingSystem.OnExorcismTyping.RemoveListener(OnExorcismTyping);
                CombatSystem.CombatTypingSystem.OnCompleteWord.RemoveListener(OnCompleteWord);
            }
            if (CombatSystem.CombatUI)
            {
                CombatSystem.CombatUI.OnFlickerStartBright.RemoveListener(OnFlickerStartBright);
            }
            if (CombatSystem.ExorcismLetterSystem)
            {
                CombatSystem.ExorcismLetterSystem.OnChangeExorcismLetter.RemoveListener(OnChangeExorcismLetter);
                CombatSystem.ExorcismLetterSystem.OnUseInCannotUseExorcismLetter.RemoveListener(OnUseInCannotUseExorcismLetter);
            }
            if (CombatSystem.ExorcismWordSystem)
            {
                CombatSystem.ExorcismWordSystem.OnExorciseWeaknessWord.RemoveListener(OnExorciseWeaknessWord);
                CombatSystem.ExorcismWordSystem.OnExorciseCurseWord.RemoveListener(OnExorciseCurseWord);
            }
        }
        if (m_WordSystem)
        {
            m_WordSystem.OnSuccessWord.RemoveListener(OnSuccessWord);
            m_WordSystem.OnFailedWord.RemoveListener(OnFailedWord);
            m_WordSystem.OnSuccessWordNotDuplicate.RemoveListener(OnSuccessWordNotDuplicate);
            m_WordSystem.OnFailedWordNotDuplicate.RemoveListener(OnFailedWordNotDuplicate);
            m_WordSystem.OnSuccessExorcismWord.RemoveListener(OnSuccessExorcismWord);
            m_WordSystem.OnFailedExorcismWord.RemoveListener(OnFailedExorcismWord);
        }
    }

    void OnTyping(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnTyping(word);
    }

    void OnChangeExorcismLetter(List<char> letters)
    {
        GhostCombatSystem.Instance.GhostAbility.OnChangeExorcismLetter(letters);
    }

    void OnExorcismTyping(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnExorcismTyping(word);
    }

    void OnCompleteWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnCompleteWord(word);
    }

    void OnSuccessWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnSuccessWord(word);
    }

    void OnFailedWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnFailedWord(word);
    }

    void OnSuccessWordNotDuplicate(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnSuccessWordNotDuplicate(word);
    }

    void OnFailedWordNotDuplicate(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnFailedWordNotDuplicate(word);
    }

    void OnSuccesLetterRule()
    {
        GhostCombatSystem.Instance.GhostAbility.OnSuccesLetterRule();
    }

    void OnFailedLetterRule()
    {
        GhostCombatSystem.Instance.GhostAbility.OnFailedLetterRule();
    }

    void OnSuccessExorcismWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnSuccessExorcismWord(word);
    }

    void OnFailedExorcismWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnFailedExorcismWord(word);
    }

    void OnExorciseWeaknessWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnExorciseWeaknessWord(word);
    }


    void OnExorciseCurseWord(string word)
    {
        GhostCombatSystem.Instance.GhostAbility.OnExorciseCurseWord(word);
    }

    void OnFlickerStartBright(int num)
    {
        GhostCombatSystem.Instance.GhostAbility.OnFlickerStartBright(num);
    }

    void OnUseInCannotUseExorcismLetter()
    {
        GhostCombatSystem.Instance.GhostAbility.OnUseInCannotUseExorcismLetter();
    }
}
