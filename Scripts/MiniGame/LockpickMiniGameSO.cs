using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiniGameSO/LockpickMiniGameSO", fileName = "LockpickMiniGameSO")]
public class LockpickMiniGameSO : MiniGameSO
{
    [SerializeField] private int m_LetterNum = 5;
    private string m_Letters;
    private int m_LetterCount;
    [SerializeField, Range(2, 6)] private int m_WordNeeded = 4;
    [SerializeField] private int[] m_WordLengthConditions;
    private float m_WordPt, m_LengthPt;
    private int m_TempWordNum;

    public override void Initialize(WordSystem wordSystem)
    {
        base.Initialize(wordSystem);

        m_LetterCount = Math.Min(m_LetterNum, GameManager.Instance.MiniGameManager.MiniGameUI.GetLockLetterBoxCount());
        for (int i = 0; i < m_LetterCount; i++)
        {
            m_Letters += m_WordSystem.GetRandomAlphabet();
        }
        CalculatePoints();
    }

    public override void StartMiniGame()
    {
        m_Letters = string.Empty;
        m_TempWordNum = 0;

        GameManager.Instance.LockingMechanic.Words.Clear();
        GameManager.Instance.LockingMechanic.WordFitConditions.Clear();
        for (int i = 0; i < m_WordNeeded; i++)
        {
            GameManager.Instance.LockingMechanic.Words.Add("");
        }
        for (int i = 0; i < m_WordLengthConditions.Length; i++)
        {
            GameManager.Instance.LockingMechanic.WordFitConditions.Add("");
        }

        GameManager.Instance.MiniGameManager.MiniGameUI.ShowLockSection(
            m_Letters,
            m_WordNeeded,
            m_WordLengthConditions
        );
    }

    private void CalculatePoints()
    {
        int wordCount = m_WordNeeded;
        int lengthCount = m_WordLengthConditions.Length;

        // Give more weight to length points
        float wordWeight = 1f;      // base weight for words
        float lengthWeight = 1.5f;  // higher weight for lengths

        // Total weight = word slots + length slots (with weights)
        float totalWeight = (wordCount * wordWeight) + (lengthCount * lengthWeight);

        // Distribute 100 proportionally
        m_WordPt = (100f * wordWeight) / totalWeight;
        m_LengthPt = (100f * lengthWeight) / totalWeight;
    }

    public override void OnFailedWord(string word)
    {
        GameManager.Instance.LockingMechanic.ReduceLockpickHealth();
    }

    public override void OnSuccessWordNotDuplicate(string word)
    {
        if (m_TempWordNum >= GameManager.Instance.LockingMechanic.Words.Count)
            GameManager.Instance.LockingMechanic.Words.Add(word);
        else
            GameManager.Instance.LockingMechanic.Words[m_TempWordNum] = word;

        for (int i = 0; i < m_WordLengthConditions.Length; i++)
        {
            if (word.Length == m_WordLengthConditions[i] && string.IsNullOrEmpty(GameManager.Instance.LockingMechanic.WordFitConditions[i]))
                GameManager.Instance.LockingMechanic.WordFitConditions[i] = word;
        }

        GameManager.Instance.MiniGameManager.MiniGameUI.UpdateLockUI();
        m_TempWordNum++;
    }

    public override void OnFailedWordNotDuplicate(string word)
    {
        OnFailedWord(word);
    }
}
