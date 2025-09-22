using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WeaknessWordSystem : MonoBehaviour
{
    [SerializeField] private CombatSystem m_CombatSystem;
    public List<string> WeaknessWords { get; private set; } = new();
    public List<string> CommonWeaknessWords { get; private set; } = new();
    public List<string> ShowCommonWeaknessWords { get; private set; } = new();
    public List<string> CurseWords { get; private set; } = new();
    public UnityEvent<List<string>> OnWeaknessWordChange, OnCurseWordChange;
    private Tween m_BanishedTween;

    void Update()
    {
        if (GameManager.Instance.IsPause) return;
        if (!m_CombatSystem.IsStart) return;
        if (m_CombatSystem.IsEnd) return;
    }

    public void StartCombat()
    {
        WeaknessWords.Clear();
        CurseWords.Clear();
        CommonWeaknessWords.Clear();
        ShowCommonWeaknessWords.Clear();

        foreach (var item in m_CombatSystem.CombatDataManager.GhostTemplate.WeaknessWords)
        {
            WeaknessWords.Add(item);
        }

        CommonWeaknessWords = WordChecking.GetCommonWords(
            m_CombatSystem.CombatDataManager.WeaknessWordCollection.WeaknessWords,
            WeaknessWords);

        ShowCommonWeaknessWords = new List<string>(CommonWeaknessWords);

        OnWeaknessWordChange?.Invoke(ShowCommonWeaknessWords);
        OnCurseWordChange?.Invoke(CurseWords);
    }

    public void UseWeaknessWord(string word)
    {
        WeaknessWords.Remove(word);
        CommonWeaknessWords.Remove(word);
        ShowCommonWeaknessWords.Remove(word);
        OnWeaknessWordChange?.Invoke(ShowCommonWeaknessWords);

        if (WeaknessWords.Count == 0)
        {
            m_BanishedTween?.Kill();
            m_BanishedTween = DOVirtual
                .DelayedCall(2f, m_CombatSystem.GhostBanished)
                .SetId(this);
        }
    }

    public void AddFakeWord(string word)
    {
        CurseWords.Add(word);
        OnCurseWordChange?.Invoke(CurseWords);
    }

    public void ClearFakeWord()
    {
        CurseWords.Clear();
        OnCurseWordChange?.Invoke(CurseWords);
    }

    public void HideWeaknessWord(int num)
    {
        if (num >= 0 && num < ShowCommonWeaknessWords.Count)
        {
            ShowCommonWeaknessWords.RemoveAt(num);
            OnWeaknessWordChange?.Invoke(ShowCommonWeaknessWords);
        }
        else
        {
            Debug.Log($"HideWeaknessWord index {num} out of range. Count: {ShowCommonWeaknessWords.Count}");
        }
    }

    public void ShowWeaknessWord(int num)
    {
        if (num >= 0 && num < CommonWeaknessWords.Count)
        {
            string wordToShow = CommonWeaknessWords[num];

            if (!ShowCommonWeaknessWords.Contains(wordToShow))
            {
                ShowCommonWeaknessWords.Add(wordToShow);
                OnWeaknessWordChange?.Invoke(ShowCommonWeaknessWords);
            }
            else
            {
                Debug.Log("no CommonWeaknessWords to show");
            }
        }
        else
        {
            Debug.Log($"ShowWeaknessWord index {num} out of range. Count: {CommonWeaknessWords.Count}");
        }
    }
}
