using UnityEngine;

[CreateAssetMenu(fileName = "HealthConsumableItemSO", menuName = "Item/HealthConsumableItemSO", order = 1)]
public class HealthConsumableItem : ConsumableItemSO
{
    [SerializeField] private HealthData m_HealthData = new();

    public override void Use(ObjectT objectT)
    {
        base.Use(objectT);
        if (objectT is Character character)
            character.ObjectHealth.Heal(m_HealthData);
    }
}
