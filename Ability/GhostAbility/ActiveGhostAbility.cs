using System.Collections.Generic;
using UnityEngine;

public class ActiveGhostAbility : GhostAbility
{
    [SerializeField] protected ActiveAbilityData m_ActiveAbilityData = new();
    [SerializeField] protected HealthData m_HealthData;
    protected float m_TextCooldown;
    [SerializeField] protected float m_TextTime = 60;
    protected CombatTypingSystem m_CombatTypingSystem;

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        if (objectT is Character character)
            character.ObjectMagic.Magic(AbilityName, Cost);
        DoActiveAbility();
        m_TextCooldown = m_TextTime;
        m_CombatTypingSystem = GhostCombatSystem.Instance.CombatSystem.CombatTypingSystem;
        if (m_CombatTypingSystem != null)
            m_CombatTypingSystem.StartSystem(true);
    }

    public override void DoUpdate()
    {
        if (GameManager.Instance.IsPause) return;
        m_TextCooldown -= Time.deltaTime;
        //Debug.Log("scale " + TextCooldown / TextTime);
        GhostCombatSystem.Instance.CombatUI.ChangeSizeStruggleModeTextCircleImageMinScaleDiff(m_TextCooldown / m_TextTime);
        GhostCombatSystem.Instance.CombatUI.ChangeSizeStruggleModeBigTextCircleImageMinScaleDiff(m_TextCooldown / m_TextTime);

        if (m_TextCooldown < 0)
        {
            m_TextCooldown = m_TextTime;
            DoPunishment();
        }
    }

    protected void DoActiveAbility()
    {
        GhostCombatSystem.Instance.DoActiveAbility(m_ActiveAbilityData);
    }

    protected virtual void NextChallenge()
    {
        Debug.Log("NextChallenge");
        m_Index++;
    }

    public override void DoPunishment()
    {
        Debug.Log("DoPunishment");
        GhostCombatSystem.Instance.CombatSystem.CombatDataManager.Player2D.ObjectHealth.TakeDamage(m_HealthData);
        GhostCombatSystem.Instance.CombatUI.ShakePlayer();
        GhostCombatSystem.Instance.ExitActiveStruggleMode();
        GhostCombatSystem.Instance.CombatSystem.OnFailedFacingStruggleMode?.Invoke();
    }

    public override void DoReward()
    {
        GhostCombatSystem.Instance.ExitActiveStruggleMode();
        GhostCombatSystem.Instance.CombatSystem.OnSuccessFacingStruggleMode?.Invoke();
    }

    public override void ExitAbility()
    {
        GhostCombatSystem.Instance.CombatUI.StruggleModeTextCircleAnimation("Flicker", false);
        // Reset typing system state
        if (m_CombatTypingSystem != null)
            m_CombatTypingSystem.StartSystem(true);
    }
}
