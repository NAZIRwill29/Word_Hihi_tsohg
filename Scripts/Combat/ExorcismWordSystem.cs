using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ExorcismWordSystem : MonoBehaviour
{
    [SerializeField] private CombatSystem m_CombatSystem;
    [SerializeField] private WordSystem m_WordSystem;
    public UnityEvent OnTypingWeakWord;
    public UnityEvent<string> OnExorciseWeaknessWord, OnExorciseCurseWord, OnExorciseOrdinaryWord;
    public IExorciseWordAbility ExorciseWordAbility;
    public HealthData m_DamageHealthData, m_HealHealthData;
    public StabilityData m_StabilityData;

    // cached delayed-call tweens so we don’t blow past capacity
    private Tween m_ExorciseWeaknessWordTween;
    private Tween m_ExorciseOrdinaryWordTween;

    private void OnEnable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWord.AddListener(CheckTypingWord);
        m_WordSystem.OnSuccessExorcismWord.AddListener(CheckExorcismWord);
    }

    private void OnDisable()
    {
        if (!m_WordSystem) return;
        m_WordSystem.OnSuccessWord.RemoveListener(CheckTypingWord);
        m_WordSystem.OnSuccessExorcismWord.RemoveListener(CheckExorcismWord);
    }

    public void StartCombat()
    {
        ExorciseWordAbility = null;
    }

    private void CheckTypingWord(string word)
    {
        if (m_CombatSystem.WeaknessWordSystem.WeaknessWords.Contains(word))
        {
            Debug.Log("typing weak word");
            OnTypingWeakWord?.Invoke();
        }
    }

    private void CheckExorcismWord(string word)
    {
        if (m_CombatSystem.WeaknessWordSystem.WeaknessWords.Contains(word))
        {
            Debug.Log("exorcise weak word");
            OnExorciseWeaknessWord?.Invoke(word);
            m_CombatSystem.WeaknessWordSystem.UseWeaknessWord(word);
            ExorciseWordAbility?.ExorciseWeaknessWord(word);
            // kill & reuse the same tween
            m_ExorciseWeaknessWordTween?.Kill();
            m_ExorciseWeaknessWordTween = DOVirtual
                .DelayedCall(1.5f, ExorciseWeaknessWord)
                .SetId(this);  // namespace it so we can kill all if needed
        }
        else if (m_CombatSystem.WeaknessWordSystem.CurseWords.Contains(word))
        {
            Debug.Log("exorcise curse word");
            OnExorciseCurseWord?.Invoke(word);
            ExorciseWordAbility?.ExorciseCurseWord(word);
        }
        else
        {
            Debug.Log("exorcise ordinary word");
            OnExorciseOrdinaryWord?.Invoke(word);
            ExorciseWordAbility?.ExorciseOrdinaryWord(word);
            m_ExorciseOrdinaryWordTween?.Kill();
            m_ExorciseOrdinaryWordTween = DOVirtual
                .DelayedCall(1.5f, ExorciseOrdinaryWord)
                .SetId(this);
        }
    }

    private void ExorciseWeaknessWord()
    {
        m_CombatSystem.CombatDataManager.Ghost.ObjectStability.Heal(m_StabilityData);
        //m_CombatSystem.CombatDataManager.Player2D.ObjectStability.Heal(m_StabilityData);
    }
    private void ExorciseOrdinaryWord()
    {
        m_CombatSystem.CombatDataManager.Player2D.ObjectHealth.Heal(m_HealHealthData);
        m_CombatSystem.CombatDataManager.Ghost.ObjectHealth.TakeDamage(m_DamageHealthData);
        GhostCombatSystem.Instance.CombatUI.ShakeGhost();
        GhostCombatSystem.Instance.CombatUI.GhostCharAnimation("Hitted");
    }

    private void OnDestroy()
    {
        // ensure no stray tweens linger after this object is gone
        DOTween.Kill(this);
    }

    public void Test(string word)
    {
        CheckExorcismWord(word);
    }
}
