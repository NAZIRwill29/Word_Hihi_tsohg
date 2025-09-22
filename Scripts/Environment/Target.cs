using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for targets in the game, incorporating health and damage.
/// </summary>
public class Target : ObjectHealth, IHealth
{
    [Tooltip("Customize rate of damage for this target")]
    [SerializeField] float m_DamageMultiplier = 1f;

    public override void TakeDamage(HealthData healthData)
    {
        ObjectStatProcessor.UpdateVariableInListWithInvokeEvent<DataNumericalVariable>(
            this,
            StatsData.DataNumVars,
            "Health",
            dataNumVar => dataNumVar.AddNumVariable *= m_DamageMultiplier
        );

        base.TakeDamage(healthData);

        // Customize any additional class-specific logic here
        // Debug.Log($"Target custom TakeDamage: {amount}");
    }
}
