using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnCollectibleCollected = delegate { };
    public static event Action<int, Item> OnItemCollected = delegate { };
    //public static event Action<int, ItemDataFlyweight> OnItemRemoved = delegate { };

    public static void CollectibleCollected()
    {
        OnCollectibleCollected();
    }

    public static void ItemCollected(int id, Item item)
    {
        OnItemCollected(id, item);
    }

    // public static void ItemRemoved(int id, ItemDataFlyweight itemData)
    // {
    //     OnItemRemoved(id, itemData);
    // }
}