using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    private ObjectT m_ObjectT;
    public List<Item> Items = new List<Item>();
    public int MaxSlots = 10;
    public UnityEvent OnInventoryChange;
    private int m_CurrentInspectItem;

    private void Awake()
    {
        m_ObjectT = GetComponent<ObjectT>();
    }

    private void OnEnable()
    {
        GameEvents.OnItemCollected += ItemCollected;
        //GameEvents.OnItemRemoved += ItemRemoved;
    }

    private void OnDisable()
    {
        GameEvents.OnItemCollected -= ItemCollected;
        //GameEvents.OnItemRemoved -= ItemRemoved;
    }

    public void ItemCollected(int id, Item item)
    {
        if (m_ObjectT.ObjectId == id)
            AddItem(item);
    }

    // public void ItemRemoved(int id, ItemDataFlyweight itemData)
    // {
    //     if (m_ObjectT.ObjectId == id)
    //         RemoveItem(itemData);
    // }

    public void AddItem(ItemDataFlyweight newItemData)
    {
        if (newItemData == null)
            return;

        AddItem(new Item { ItemDataFlyweight = newItemData, quantity = 1 }); // avoid shared state
    }

    public void AddItem(Item newItem)
    {
        if (newItem?.ItemDataFlyweight == null)
            return;

        Item existingItem = Items.Find(item => item.ItemDataFlyweight.NameData.Name == newItem.ItemDataFlyweight.NameData.Name);

        if (existingItem != null)
        {
            existingItem.quantity += newItem.quantity;
        }
        else
        {
            if (Items.Count < MaxSlots)
            {
                Items.Add(newItem);
            }
            else
            {
                Debug.LogWarning("Inventory full!");
                return;
            }
        }

        OnInventoryChange.Invoke();
    }

    public void RemoveItem(ItemDataFlyweight itemToRemoveData)
    {
        if (itemToRemoveData == null)
            return;

        RemoveItem(new Item { ItemDataFlyweight = itemToRemoveData }, 1);
    }

    public void RemoveItem(Item itemToRemove, int num)
    {
        if (itemToRemove?.ItemDataFlyweight == null)
            return;

        Item existingItem = Items.Find(item => item.ItemDataFlyweight.NameData.Name == itemToRemove.ItemDataFlyweight.NameData.Name);

        if (existingItem != null)
        {
            existingItem.quantity -= num;
            if (existingItem.quantity <= 0)
            {
                Items.Remove(existingItem);
            }

            OnInventoryChange.Invoke();
        }
    }

    public ItemDataFlyweight InspectItem(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            m_CurrentInspectItem = index;
            return Items[index].ItemDataFlyweight;
        }

        Debug.LogWarning("InspectItem: index out of range");
        return null;
    }

    public void UseItem()
    {
        if (m_CurrentInspectItem < 0 || m_CurrentInspectItem >= Items.Count)
            return;

        if (Items[m_CurrentInspectItem].ItemDataFlyweight is ConsumableItemSO consumableItemSO)
        {
            Debug.Log("Use item");
            consumableItemSO.Use(m_ObjectT);
            RemoveItem(Items[m_CurrentInspectItem], 1);
        }
    }

    public void DropItem(GameObject gameObject)
    {
        if (m_CurrentInspectItem < 0 || m_CurrentInspectItem >= Items.Count)
            return;

        Item itemToDrop = Items[m_CurrentInspectItem];
        Debug.Log($"Dropped item: {itemToDrop.ItemDataFlyweight.NameData.Name}");

        if (GameManager.Instance.ObjectPool.GetPooledObject(itemToDrop.ItemDataFlyweight.NameData.Name) is ItemInteractable itemInteractable)
        {
            itemInteractable.Show(gameObject);
        }

        RemoveItem(itemToDrop, 1);
    }

    public bool CheckItemExist(string name)
    {
        Item item = Items.Find(x => x.ItemDataFlyweight.NameData.Name == name);
        return item != null;
    }
}
