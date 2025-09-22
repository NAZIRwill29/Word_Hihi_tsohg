using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatTypingSystem : TypingSystem
{
    [SerializeField] private CombatSystem m_CombatSystem;
    [ReadOnly, SerializeField] private string m_CurrentExorcismWord = "";
    private List<int> m_TempAvailableIndices = new();

    public UnityEvent<string> OnExorcismTyping;

    protected override void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!m_IsStart) return;
        if (m_CombatSystem == null || !m_CombatSystem.IsStart || string.IsNullOrEmpty(Input.inputString) || m_CombatSystem.IsEnd)
            return;

        foreach (char c in Input.inputString)
        {
            // NORMAL TYPING MODE
            if (m_CombatSystem.IsTypingMode)
            {
                bool changed = false;

                if (char.IsLetter(c) && KeyboardManager.Instance != null && !KeyboardManager.Instance.CheckIsBanned(c))
                {
                    m_CurrentWord += char.ToUpper(c);
                    changed = true;
                }
                else if (c == '\b' && m_CurrentWord.Length > 0)
                {
                    if (!m_IsCanBackspace)
                    {
                        OnCantBackspace.Invoke();
                        return;
                    }
                    m_CurrentWord = m_CurrentWord.Substring(0, m_CurrentWord.Length - 1);
                    changed = true;
                }

                if (changed)
                    OnTyping?.Invoke(m_CurrentWord);
            }
            // EXORCISM TYPING MODE
            else
            {
                bool changed = false;

                if (char.IsLetter(c)
                    && KeyboardManager.Instance != null
                    && GhostCombatSystem.Instance != null
                    && GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.CheckIsCanUseExorcismLetter())
                {
                    if (m_CombatSystem.ExorcismLetterSystem.UseExorcismLetter(c))
                    {
                        m_CurrentExorcismWord += char.ToUpper(c);
                        changed = true;
                    }
                }
                else if (c == '\b' && m_CurrentExorcismWord.Length > 0)
                {
                    if (!m_IsCanBackspace)
                    {
                        OnCantBackspace.Invoke();
                        return;
                    }
                    char last = m_CurrentExorcismWord[^1];
                    m_CombatSystem.ExorcismLetterSystem.RewardExorcismLetter(last);
                    m_CurrentExorcismWord = m_CurrentExorcismWord.Substring(0, m_CurrentExorcismWord.Length - 1);
                    changed = true;
                }

                if (changed)
                    OnExorcismTyping?.Invoke(m_CurrentExorcismWord);
            }
        }
    }

    public override void StartSystem(bool isTrue)
    {
        base.StartSystem(isTrue);
        m_CurrentExorcismWord = "";
        OnExorcismTyping?.Invoke(m_CurrentExorcismWord);
    }

    public override void InitConfirmWord()
    {
        if (m_CombatSystem == null || m_WordSystem == null)
            return;

        // CONFIRM NORMAL WORD
        if (m_CombatSystem.IsTypingMode)
        {
            if (!string.IsNullOrWhiteSpace(m_CurrentWord))
            {
                Debug.Log("Typed word: " + m_CurrentWord);
                OnCompleteWord?.Invoke(m_CurrentWord);
                m_WordSystem.ConfirmWord(m_CurrentWord);
                m_CurrentWord = "";
                OnTyping?.Invoke(m_CurrentWord);
            }
        }
        // CONFIRM EXORCISM WORD
        else
        {
            if (!string.IsNullOrWhiteSpace(m_CurrentExorcismWord))
            {
                Debug.Log("Typed Exorcism word: " + m_CurrentExorcismWord);
                OnCompleteWord?.Invoke(m_CurrentExorcismWord);

                if (m_WordSystem.ConfirmExorcismWord(m_CurrentExorcismWord))
                {
                    m_CurrentExorcismWord = "";
                    OnExorcismTyping?.Invoke(m_CurrentExorcismWord);
                }
            }
        }
    }

    public void OnLowStability(string word)
    {
        m_IsCanBackspace = false;
    }

    public void OnHighStability(string word)
    {
        m_IsCanBackspace = true;
    }

    public void ChangeLetterInTypingWordRandomly(int num)
    {
        if (m_CombatSystem.IsTypingMode)
        {
            if (string.IsNullOrEmpty(m_CurrentWord)) return;

            char[] chars = m_CurrentWord.ToCharArray();
            int length = chars.Length;
            num = Mathf.Min(num, length); // Clamp num to avoid overflow

            m_TempAvailableIndices.Clear();
            for (int i = 0; i < length; i++) m_TempAvailableIndices.Add(i);

            for (int i = 0; i < num; i++)
            {
                int randomListIndex = UnityEngine.Random.Range(0, m_TempAvailableIndices.Count);
                int index = m_TempAvailableIndices[randomListIndex];
                m_TempAvailableIndices.RemoveAt(randomListIndex);

                char letter = m_WordSystem.GetRandomAlphabet();
                chars[index] = letter;
            }

            m_CurrentWord = new string(chars);
            OnTyping?.Invoke(m_CurrentWord);
        }
        else
        {
            if (string.IsNullOrEmpty(m_CurrentExorcismWord)) return;

            char[] chars = m_CurrentExorcismWord.ToCharArray();
            int length = chars.Length;
            num = Mathf.Min(num, length); // Clamp num to avoid overflow

            m_TempAvailableIndices.Clear();
            for (int i = 0; i < length; i++) m_TempAvailableIndices.Add(i);

            for (int i = 0; i < num; i++)
            {
                int randomListIndex = UnityEngine.Random.Range(0, m_TempAvailableIndices.Count);
                int index = m_TempAvailableIndices[randomListIndex];
                m_TempAvailableIndices.RemoveAt(randomListIndex);

                char letter = m_WordSystem.GetRandomAlphabet();
                chars[index] = letter;
            }

            m_CurrentExorcismWord = new string(chars);
            OnExorcismTyping?.Invoke(m_CurrentExorcismWord);
        }
    }

    public void ChangeTypingWordRandomly()
    {
        int lengthMin = Math.Max(m_CurrentWord.Length - 2, 4);
        int lengthMax = Math.Min(m_CurrentWord.Length + 3, 4);
        string word = m_CombatSystem.WordCheck.GetRandomWord(lengthMin, lengthMax);
        word = word.Substring(0, word.Length - 2);
        m_CurrentWord = word;
    }
}
