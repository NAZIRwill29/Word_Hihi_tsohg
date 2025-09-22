using UnityEngine;

public class ItemCollectible : Collectible
{
    //[SerializeField] private int m_ItemID;
    [SerializeField] private Item m_Item;

    protected override void ThingHappen(ObjectT objectT)
    {
        string soundName = SoundNameData != null ? SoundNameData.Name : "";
        string fxName = FXNameData != null ? FXNameData.Name : "";

        objectT.ThingHappen(new() { SoundName = soundName, FXName = fxName });

        // Notify any listeners through the event channel
        GameEvents.CollectibleCollected();
        GameEvents.ItemCollected(objectT.ObjectId, m_Item);

        Destroy(gameObject);
    }
}
