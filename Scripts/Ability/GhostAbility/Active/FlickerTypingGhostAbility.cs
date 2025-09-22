using UnityEngine;

[CreateAssetMenu(fileName = "FlickerTypingGhostAbility", menuName = "Abilities/Ghost/FlickerTypingGhostAbility")]
public class FlickerTypingGhostAbility : ActiveGhostAbility
{
    [SerializeField] private float m_FlickerTime = 0.3f;
    [SerializeField] private int m_FlickerNum = 1;
    [SerializeField] private float m_MaxAlphaTime = 2;
    [SerializeField] private float m_MinAlphaTime = 2;
    [SerializeField] private HealthData m_ExtraHealthData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here

        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(m_Words[m_Index]);
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", true);

        //TODO()
        GhostCombatSystem.Instance.CombatUI.SetStruggleModeFlicker(0, m_FlickerNum);
        GhostCombatSystem.Instance.CombatUI.SetSrambleTextFlickerTime(m_FlickerTime, m_MaxAlphaTime, m_MinAlphaTime);
    }

    public override void OnTyping(string word)
    {
        if (!GhostCombatSystem.Instance.CombatUI.CheckFlickerIsBright(0))
        {
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("RedFlashTrig");
            GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_ExtraHealthData);
            GhostCombatSystem.Instance.CombatUI.ShakePlayer();
        }
    }

    public override void OnSuccessWord(string word)
    {
        if (m_Words[m_Index] == word)
            DoReward();
    }

    public override void OnFlickerStartBright(int num)
    {
        if (num == 0)
            GhostCombatSystem.Instance.CombatUI.EffectImageAnimation("ElectricPulseTrig");
    }

    public override void ExitAbility()
    {
        base.ExitAbility();
        m_Words.Clear();
        GhostCombatSystem.Instance.CombatUI.ChangeStruggleModeText(string.Empty);
        GhostCombatSystem.Instance.CombatUI.ReseFlickerUI();
    }
}
