using UnityEngine;

public class PassiveGhostAbility : GhostAbility
{
    [SerializeField] protected PassiveAbilityData m_PassiveAbilityData = new();

    public override void Use(ObjectT objectT, ExecuteActionCommandData data = null)
    {
        base.Use(objectT, data);
        // If additional use logic is needed, add here
        DoPassivebility();
    }

    protected void DoPassivebility()
    {
        GhostCombatSystem.Instance.DoPassivebility(m_PassiveAbilityData);
    }
}
