using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "LetterSnatcherGhostAbility", menuName = "Abilities/Ghost/LetterSnatcherGhostAbility")]
public class LetterSnatcherGhostAbility : PassiveGhostAbility
{
    private int m_StealLetterNum;
    [SerializeField] private int m_StealLetterFrequentNum = 1;
    [SerializeField] private int m_StealLetterNumMax = 6;
    [SerializeField] private List<string> m_RestorativeWords;
    private List<string> m_TempRestorativeWords = new();
    private List<string> m_RemoveRestorativeWords = new();
    private float m_StealCooldown;
    [SerializeField] private float m_StealTime = 15;

    // cached delayed-call tweens so we don’t blow past capacity
    private Tween m_StealExorcismLetterAnimationTween;
    private Tween m_StealExorcismLetterOnceTween;
    private Tween m_RetrieveHiddenExorcismLetterTween;
    private Tween m_StealExorcismLetterFrequentTween;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_StealCooldown = m_StealTime;
        m_RemoveRestorativeWords.Clear();
        m_TempRestorativeWords.Clear();
        foreach (var item in m_RestorativeWords)
        {
            m_TempRestorativeWords.Add(item);
        }

        GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_TempRestorativeWords, true);

        int max = Math.Min(m_StealLetterNumMax, GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ExorcismLetters.Count - 2);
        int min = Math.Max(1, max - 2);
        m_StealLetterNum = UnityEngine.Random.Range(min, max);

        // kill & reuse the same tween
        m_StealExorcismLetterAnimationTween?.Kill();
        m_StealExorcismLetterAnimationTween = DOVirtual
            .DelayedCall(2f, StealExorcismLetterAnimation)
            .SetId(this);  // namespace it so we can kill all if needed

        // kill & reuse the same tween
        m_StealExorcismLetterOnceTween?.Kill();
        m_StealExorcismLetterOnceTween = DOVirtual
            .DelayedCall(3f, StealExorcismLetterOnce)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        base.DoUpdate();
        m_StealCooldown -= Time.deltaTime;
        if (m_StealCooldown <= 0)
        {
            m_StealCooldown = m_StealTime;
            DoPunishment();
            m_TempRestorativeWords.Add(m_RemoveRestorativeWords[0]);
            m_RemoveRestorativeWords.RemoveAt(0);
            GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_TempRestorativeWords, true);
        }
    }

    public override void OnSuccessWord(string word)
    {
        if (WordChecking.CheckIsWordExistInList(m_RestorativeWords, word))
        {
            DoReward();
            m_TempRestorativeWords.Remove(word);
            m_RemoveRestorativeWords.Add(word);
            GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_TempRestorativeWords, true);
        }
    }

    public override void DoReward()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImage2Animation("HandGiveTrig");
        // kill & reuse the same tween
        m_RetrieveHiddenExorcismLetterTween?.Kill();
        m_RetrieveHiddenExorcismLetterTween = DOVirtual
            .DelayedCall(1f, GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.RetrieveHiddenExorcismLetter)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    public override void DoPunishment()
    {
        m_StealExorcismLetterAnimationTween?.Kill();
        m_StealExorcismLetterAnimationTween = DOVirtual
            .DelayedCall(2f, StealExorcismLetterAnimation)
            .SetId(this);  // namespace it so we can kill all if needed
        m_StealExorcismLetterFrequentTween?.Kill();
        m_StealExorcismLetterFrequentTween = DOVirtual
            .DelayedCall(3f, StealExorcismLetterFrequent)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    protected virtual void StealExorcismLetterAnimation()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("HandStealTrig");
    }

    private void StealExorcismLetterOnce()
    {
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.HideExorcismLetter(m_StealLetterNum);
    }

    private void StealExorcismLetterFrequent()
    {
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.HideExorcismLetter(m_StealLetterFrequentNum);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_StealCooldown = m_StealTime;
        GhostCombatSystem.Instance.CombatUI.ChangeTopCenterTexts(m_RestorativeWords, false);
    }
}
