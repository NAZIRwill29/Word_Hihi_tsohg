using System;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceMimicModeGhostAbility", menuName = "Abilities/Ghost/VoiceMimicModeGhostAbility")]
public class VoiceMimicModeGhostAbility : PassiveGhostAbility, IExorciseWordAbility
{
    [SerializeField] private WordSystem m_WordSystem;
    [SerializeField] private HealthData m_HealthData;

    private Tween m_DoPunishmentTween, m_ChangeWordTween, m_GlitchAnimationTween, m_DoRewardTween;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        // Clear existing tweens
        m_GlitchAnimationTween?.Kill();
        m_ChangeWordTween?.Kill();

        m_GlitchAnimationTween = DOVirtual.DelayedCall(2f, GlitchAnimation).SetId(this);
        m_ChangeWordTween = DOVirtual.DelayedCall(2.4f, ChangeWord).SetId(this);

        GhostCombatSystem.Instance.CombatSystem.ExorcismWordSystem.ExorciseWordAbility = this;
    }

    private void GlitchAnimation()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
    }

    private void ChangeWord()
    {
        var weaknessSystem = GhostCombatSystem.Instance.CombatSystem.WeaknessWordSystem;
        var weaknessWords = weaknessSystem.CommonWeaknessWords;

        weaknessSystem.ClearFakeWord();

        // Cache count
        int count = weaknessWords.Count;

        for (int i = 0; i < count; i++)
        {
            char[] charArray = weaknessWords[i].ToCharArray();
            int randomNum = UnityEngine.Random.Range(0, charArray.Length - 1);
            charArray[randomNum] = m_WordSystem.GetRandomAlphabet();

            m_Words.Add(new string(charArray));
            weaknessSystem.AddFakeWord(m_Words[i]);
        }

        // Loop backward to avoid index shift issues if HideWeaknessWord modifies the list
        for (int i = count - 1; i >= 0; i--)
        {
            weaknessSystem.HideWeaknessWord(i);
        }
        GhostCombatSystem.Instance.CombatUI.CurseWordAnimations("Blink", true);
    }

    public void ExorciseCurseWord(string word)
    {
        //GhostCombatSystem.Instance.CombatUI.CurseWordAnimation(word, "BlinkTrig");

        m_DoPunishmentTween?.Kill();
        m_DoPunishmentTween = DOVirtual.DelayedCall(1.5f, DoPunishment).SetId(this);
    }

    public void ExorciseWeaknessWord(string word)
    {
        Debug.Log("ExorciseWeaknessWord in ability");
        GlitchAnimation();

        m_DoRewardTween?.Kill();
        m_DoRewardTween = DOVirtual.DelayedCall(1.5f, DoReward).SetId(this);
    }

    public void ExorciseOrdinaryWord(string word)
    {
        // GhostCombatSystem.Instance.CombatUI.CurseWordAnimation(word, "BlinkTrig");

        // m_DoPunishmentTween?.Kill();
        // m_DoPunishmentTween = DOVirtual.DelayedCall(1.5f, DoPunishment).SetId(this);
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_HealthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }

    public override void DoReward()
    {
        var shownWords = GhostCombatSystem.Instance.CombatSystem.WeaknessWordSystem.CommonWeaknessWords;
        if (shownWords.Count == 0) return;

        int index = UnityEngine.Random.Range(0, shownWords.Count);
        GhostCombatSystem.Instance.CombatSystem.WeaknessWordSystem.ShowWeaknessWord(index);
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        GhostCombatSystem.Instance.CombatUI.CurseWordAnimations("Blink", false);
    }
}
