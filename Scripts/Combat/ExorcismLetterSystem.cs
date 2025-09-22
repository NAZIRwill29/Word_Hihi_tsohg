using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExorcismLetterSystem : MonoBehaviour
{
    [SerializeField] private WordSystem m_WordSystem;
    [ReadOnly] public List<char> ExorcismLetters = new();
    [ReadOnly] public List<char> FakeExorcismLetters = new();
    [ReadOnly] public List<char> TempExorcismLetters = new();
    [SerializeField] private LetterPlacementSystem m_LetterPlacementSystem;
    public List<char> HiddenExorcismLetters { get; private set; } = new();
    private List<int> m_FakeExorcismLetterIndexes = new();
    public bool IsCanUseExorcismLetter = true;
    private List<int> m_TempAvailableIndices = new();
    public UnityEvent<List<char>> OnChangeExorcismLetter;
    public UnityEvent OnUseInCannotUseExorcismLetter;

    private void OnEnable()
    {
        if (m_LetterPlacementSystem != null)
            m_LetterPlacementSystem.OnRewardLetterPlacement.AddListener(RewardExorcismLetter);
    }

    private void OnDisable()
    {
        if (m_LetterPlacementSystem != null)
            m_LetterPlacementSystem.OnRewardLetterPlacement.RemoveListener(RewardExorcismLetter);
    }

    public void StartCombat()
    {
        ExorcismLetters.Clear();
        FakeExorcismLetters.Clear();
        TempExorcismLetters.Clear();
        HiddenExorcismLetters.Clear();
        m_FakeExorcismLetterIndexes.Clear();
        IsCanUseExorcismLetter = true;
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }

    public bool CheckIsCanUseExorcismLetter()
    {
        if (!IsCanUseExorcismLetter)
        {
            //Debug.Log("OnUseInCannotUseExorcismLetter");
            OnUseInCannotUseExorcismLetter?.Invoke();
        }
        return IsCanUseExorcismLetter;
    }

    public void RewardExorcismLetter(char letter)
    {
        if (GhostCombatSystem.Instance.ActiveStruggleMode) return;

        ExorcismLetters.Add(letter);
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }

    public bool UseExorcismLetter(char letter)
    {
        if (!IsCanUseExorcismLetter)
            return false;

        int idx = ExorcismLetters.IndexOf(letter);
        if (idx >= 0)
        {
            //Debug.Log("UseExorcismLetter " + letter);
            ExorcismLetters.RemoveAt(idx);
            OnChangeExorcismLetter?.Invoke(ExorcismLetters);
            RemoveFakeExorcismLetter();
            return true;
        }
        return false;
    }

    #region Fake

    public void SetFakeExorcismLetters(int count)
    {
        FakeExorcismLetters.Clear();
        m_FakeExorcismLetterIndexes.Clear();

        int countToFake = Math.Min(count, ExorcismLetters.Count);
        for (int i = 0; i < countToFake; i++)
        {
            FakeExorcismLetters.Add(m_WordSystem.GetRandomAlphabet());
            m_FakeExorcismLetterIndexes.Add(i);
        }
        SetTempExorcismLetters();
    }

    public void SetFakeExorcismLetter(int num)
    {
        if (num < 0 || num > ExorcismLetters.Count)
            return;

        FakeExorcismLetters.Add(m_WordSystem.GetRandomAlphabet());
        m_FakeExorcismLetterIndexes.Add(num);
        SetTempExorcismLetters();
    }

    private void RemoveFakeExorcismLetter()
    {
        if (FakeExorcismLetters.Count > 0)
        {
            FakeExorcismLetters.RemoveAt(FakeExorcismLetters.Count - 1);
        }
        if (m_FakeExorcismLetterIndexes.Count > 0)
        {
            m_FakeExorcismLetterIndexes.RemoveAt(m_FakeExorcismLetterIndexes.Count - 1);
        }
        SetTempExorcismLetters();
    }

    public void ResetFakeExorcismLetter()
    {
        FakeExorcismLetters.Clear();
        TempExorcismLetters.Clear();
        m_FakeExorcismLetterIndexes.Clear();
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }

    public void SetTempExorcismLetters()
    {
        TempExorcismLetters.Clear();
        TempExorcismLetters.AddRange(ExorcismLetters);
        for (int i = 0; i < m_FakeExorcismLetterIndexes.Count; i++)
        {
            int index = m_FakeExorcismLetterIndexes[i];
            if (index >= 0 && index < TempExorcismLetters.Count)
            {
                TempExorcismLetters[index] = FakeExorcismLetters[i];
            }
        }
    }

    public void ShowFakeExorcismLetter(bool isTrue)
    {
        OnChangeExorcismLetter?.Invoke(isTrue ? TempExorcismLetters : ExorcismLetters);
    }

    #endregion

    #region Hide

    public void HideExorcismLetter(int count)
    {
        int countToHide = Math.Min(count, ExorcismLetters.Count);
        for (int i = 0; i < countToHide; i++)
        {
            int index = UnityEngine.Random.Range(0, ExorcismLetters.Count);
            char letter = ExorcismLetters[index];
            HiddenExorcismLetters.Add(letter);
            ExorcismLetters.RemoveAt(index);
        }
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }

    public void RetrieveHiddenExorcismLetter()
    {
        if (HiddenExorcismLetters.Count == 0)
            return;

        int hiddenIndex = UnityEngine.Random.Range(0, HiddenExorcismLetters.Count);
        char letter = HiddenExorcismLetters[hiddenIndex];
        HiddenExorcismLetters.RemoveAt(hiddenIndex);
        ExorcismLetters.Add(letter);
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }

    #endregion

    public void ChangeLetterInExorcismLettersRandomly(int num)
    {
        if (ExorcismLetters.Count == 0) return;

        int length = ExorcismLetters.Count;
        num = Mathf.Min(num, length); // Clamp num to avoid overflow

        m_TempAvailableIndices.Clear();
        for (int i = 0; i < length; i++) m_TempAvailableIndices.Add(i);

        for (int i = 0; i < num; i++)
        {
            int randomListIndex = UnityEngine.Random.Range(0, m_TempAvailableIndices.Count);
            int index = m_TempAvailableIndices[randomListIndex];
            m_TempAvailableIndices.RemoveAt(randomListIndex);

            ExorcismLetters[index] = m_WordSystem.GetRandomAlphabet();
        }
        OnChangeExorcismLetter?.Invoke(ExorcismLetters);
    }
}
