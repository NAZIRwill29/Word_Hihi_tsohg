using UnityEngine;

[CreateAssetMenu(fileName = "StabilityConsumableItemSO", menuName = "Item/StabilityConsumableItemSO", order = 1)]
public class StabilityConsumableItem : ConsumableItemSO
{
    [SerializeField] private StabilityData m_StabilityData = new();

    public override void Use(ObjectT objectT)
    {
        base.Use(objectT);
        if (objectT is Character character)
            character.ObjectStability.Heal(m_StabilityData);
    }
}
