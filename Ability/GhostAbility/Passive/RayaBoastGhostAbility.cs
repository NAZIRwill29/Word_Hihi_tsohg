using DG.Tweening;
using UnityEngine;

/// <summary>
/// sound
/// </summary>
[CreateAssetMenu(fileName = "RayaBoastGhostAbility", menuName = "Abilities/Ghost/RayaBoastGhostAbility")]
public class RayaBoastGhostAbility : PassiveGhostAbility
{
    private Tween m_DoPunishmentTween, m_GlitchAnimationTween;
    //[SerializeField] private float m_RayaTimeMin = 2.5f, m_RayaTimeMax = 5;
    //private float m_RayaCooldown;
    //[SerializeField] private ThingHappenData m_ThingHappenData = new();

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        //m_RayaCooldown = Random.Range(m_RayaTimeMin, m_RayaTimeMax);
        //GhostCombatSystem.Instance.CombatSystem.GhostT.ThingHappen(m_ThingHappenData);
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

    // public override void DoUpdate()
    // {
    //     m_RayaCooldown -= Time.deltaTime;
    //     if (m_RayaCooldown <= 0)
    //     {
    //         GhostCombatSystem.Instance.CombatSystem.GhostT.ThingHappen(m_ThingHappenData);
    //         GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
    //         DOVirtual.DelayedCall(0.5f, DoPunishment);
    //         m_RayaCooldown = Random.Range(m_RayaTimeMin, m_RayaTimeMax);
    //     }
    // }

    public override void DoPunishment()
    {
        int num = Random.Range(1, 3);
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ChangeLetterInExorcismLettersRandomly(num);
    }

    // public override void ExitAbility()
    // {
    //     base.ExitAbility();
    //     m_RayaCooldown = Random.Range(m_RayaTimeMin, m_RayaTimeMax);
    // }
}
