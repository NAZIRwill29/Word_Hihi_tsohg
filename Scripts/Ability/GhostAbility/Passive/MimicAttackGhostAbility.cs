using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MimicAttackGhostAbility", menuName = "Abilities/Ghost/MimicAttackGhostAbility")]
public class MimicAttackGhostAbility : PassiveGhostAbility, IExorciseWordAbility
{
    [SerializeField] private HealthData m_HealthData;
    // cached delayed-call tweens so we don’t blow past capacity
    private Tween m_DoPunishmentTween, m_AddFakeWordTween, m_GlitchAnimationTween;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_GlitchAnimationTween?.Kill();
        m_GlitchAnimationTween = DOVirtual
            .DelayedCall(2f, GlitchAnimation)
            .SetId(this);  // namespace it so we can kill all if needed
        m_AddFakeWordTween?.Kill();
        m_AddFakeWordTween = DOVirtual
            .DelayedCall(2.4f, AddFakeWord)
            .SetId(this);  // namespace it so we can kill all if needed

        GhostCombatSystem.Instance.CombatSystem.ExorcismWordSystem.ExorciseWordAbility = this;
    }

    private void GlitchAnimation()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
    }

    private void AddFakeWord()
    {
        GhostCombatSystem.Instance.CombatSystem.WeaknessWordSystem.ClearFakeWord();
        GhostCombatSystem.Instance.CombatSystem.WeaknessWordSystem.AddFakeWord(m_Words[m_Index]);
    }

    public void ExorciseCurseWord(string word)
    {
        //GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("ScreenCrackTrig");
        GhostCombatSystem.Instance.CombatUI.CurseWordAnimation(word, "BlinkTrig");
        // kill & reuse the same tween
        m_DoPunishmentTween?.Kill();
        m_DoPunishmentTween = DOVirtual
            .DelayedCall(1.5f, DoPunishment)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_HealthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
    }

    public void ExorciseWeaknessWord(string word)
    {
    }

    public void ExorciseOrdinaryWord(string word)
    {
    }
}
