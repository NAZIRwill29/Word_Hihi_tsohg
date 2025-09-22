using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "Item/ConsumableItemSO", order = 1)]
public class ConsumableItemSO : DropableItemSO
{
    [SerializeField] protected ThingHappenData m_ThingHappenData;
    public string PositiveInItemText = "Use";

    public virtual void Use(ObjectT objectT)
    {
        objectT.ThingHappen(m_ThingHappenData);
        //GameEvents.ItemRemoved(objectT.ObjectId, this);
        //do something
    }
}
