using UnityEngine;

public class HealthBoost : PowerUp
{

    [SerializeField] private HealthData m_HealthData;
    public override void ApplyEffect(GameObject character)
    {
        ObjectHealth characterHealth = character.GetComponent<ObjectHealth>();

        if (characterHealth != null)
        {
            characterHealth.Heal(m_HealthData);
        }
    }
}
