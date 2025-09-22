using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "StealingBeautyGhostAbility", menuName = "Abilities/Ghost/StealingBeautyGhostAbility")]
public class StealingBeautyGhostAbility : LetterSnatcherGhostAbility
{
    // cached delayed-call tweens so we don’t blow past capacity
    private Tween m_RetrieveHiddenExorcismLetterTween;
    public override void DoReward()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImage2Animation("BeautyGiveTrig");
        // kill & reuse the same tween
        m_RetrieveHiddenExorcismLetterTween?.Kill();
        m_RetrieveHiddenExorcismLetterTween = DOVirtual
            .DelayedCall(1f, GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.RetrieveHiddenExorcismLetter)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    protected override void StealExorcismLetterAnimation()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("BeautyStealTrig");
    }
}
