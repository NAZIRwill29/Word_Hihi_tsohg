using DG.Tweening;
using UnityEngine;

/// <summary>
/// GhostProfSprite, GhostCharSprite, sound
/// </summary>
[CreateAssetMenu(fileName = "TerrifyingRevealGhostAbility", menuName = "Abilities/Ghost/TerrifyingRevealGhostAbility")]
public class TerrifyingRevealGhostAbility : PassiveGhostAbility
{
    // [SerializeField] private float m_MinusTime = -0.2f;
    private Tween m_DoPunishmentTween, m_GlitchAnimationTween;
    [SerializeField] private MagicData healMagicData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_GlitchAnimationTween?.Kill();
        m_GlitchAnimationTween = DOVirtual
            .DelayedCall(2f, GlitchAnimation)
            .SetId(this);  // namespace it so we can kill all if needed
        m_DoPunishmentTween?.Kill();
        m_DoPunishmentTween = DOVirtual
            .DelayedCall(2.4f, DoPunishment)
            .SetId(this);  // namespace it so we can kill all if needed
    }

    private void GlitchAnimation()
    {
        GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
    }

    public override void DoPunishment()
    {
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ChangeLetterInExorcismLettersRandomly(
            GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ExorcismLetters.Count
            );
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Ghost.ObjectMagic.Heal(healMagicData);
    }
}
