using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FormShiftGhostAbility", menuName = "Abilities/Ghost/FormShiftGhostAbility")]
public class FormShiftGhostAbility : PassiveGhostAbility
{
    private float m_RealCooldown, m_FakeCooldown;
    [SerializeField] private float m_RealTime = 1.5f, m_FakeTime = 2;
    private bool m_InFake;
    [SerializeField] private StabilityData m_StabilityData;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        m_InFake = false;
        m_FakeCooldown = 0;
        m_RealCooldown = 0;
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.SetFakeExorcismLetters(GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ExorcismLetters.Count);
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        float delta = Time.deltaTime;

        if (m_InFake)
        {
            m_RealCooldown -= delta;
            if (m_RealCooldown <= 0)
            {
                // Switch to real
                GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
                GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ShowFakeExorcismLetter(false);
                m_FakeCooldown = m_FakeTime;
                m_InFake = false;
                GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.IsCanUseExorcismLetter = true;
            }
        }
        else
        {
            m_FakeCooldown -= delta;
            if (m_FakeCooldown <= 0)
            {
                // Switch to fake
                GhostCombatSystem.Instance.CombatUI.WordContainerEffectImageAnimation("GlitchTrig");
                GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ShowFakeExorcismLetter(true);
                m_RealCooldown = m_RealTime;
                m_InFake = true;
                GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.IsCanUseExorcismLetter = false;
            }
        }
    }

    public override void OnChangeExorcismLetter(List<char> letters)
    {
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.SetFakeExorcismLetters(letters.Count);
    }

    public override void OnUseInCannotUseExorcismLetter()
    {
        if (m_InFake)
        {
            DoPunishment();
            // if (WordChecking.CheckContainLetter(
            //     GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ExorcismLetters,
            //     word[^1].ToString()))
            // {
            //     GhostCombatSystem.Instance.CombatSystem.TypingSystem.OnExorcismTyping
            // }
        }
    }

    public override void DoPunishment()
    {
        Debug.Log("DoPunishment");
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectStability.TakeDamage(m_StabilityData);
    }

    public override void ExitAbility()
    {
        m_FakeCooldown = 0;
        m_RealCooldown = 0;
        m_InFake = false;
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.IsCanUseExorcismLetter = true;
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ShowFakeExorcismLetter(false);
        GhostCombatSystem.Instance.CombatSystem.ExorcismLetterSystem.ResetFakeExorcismLetter();
    }
}
