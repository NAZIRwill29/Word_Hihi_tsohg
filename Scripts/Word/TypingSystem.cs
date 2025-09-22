using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class TypingSystem : MonoBehaviour
{
    [SerializeField] protected WordSystem m_WordSystem;
    [ReadOnly, SerializeField] protected string m_CurrentWord = "";
    public UnityEvent<string> OnTyping, OnCompleteWord;
    public UnityEvent OnCantBackspace;
    protected bool m_IsCanBackspace = true;
    protected bool m_IsStart;

    protected virtual void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!m_IsStart) return;

        foreach (char c in Input.inputString)
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
    }

    public virtual void StartSystem(bool isTrue)
    {
        m_IsStart = isTrue;
        m_IsCanBackspace = true;
        m_CurrentWord = "";
        if (isTrue)
            OnTyping?.Invoke(m_CurrentWord);
    }

    public virtual void InitConfirmWord()
    {
        if (m_WordSystem == null)
            return;

        if (!string.IsNullOrWhiteSpace(m_CurrentWord))
        {
            Debug.Log("Typed word: " + m_CurrentWord);
            OnCompleteWord?.Invoke(m_CurrentWord);
            m_WordSystem.ConfirmWord(m_CurrentWord);
            m_CurrentWord = "";
            OnTyping?.Invoke(m_CurrentWord);
        }
    }
}
