using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory", order = 1)]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private int maxSlots = 10;
    private Transform m_InventoryUI;
    private GameObject m_ItemSlotPrefab;

    public void Initialize(Transform inventoryUI, GameObject itemSlotPrefab)
    {
        m_InventoryUI = inventoryUI;
        m_ItemSlotPrefab = itemSlotPrefab;
    }

    public void AddItem(Item newItem)
    {
        Item existingItem = items.Find(item => item.ItemDataFlyweight.NameData.Name == newItem.ItemDataFlyweight.NameData.Name);

        if (existingItem != null)
        {
            existingItem.quantity += newItem.quantity;
        }
        else
        {
            if (items.Count < maxSlots)
            {
                items.Add(newItem);
            }
            else
            {
                Debug.Log("Inventory full!");
                return;
            }
        }
        UpdateUI();
    }

    public void RemoveItem(Item itemToRemove)
    {
        Item existingItem = items.Find(item => item.ItemDataFlyweight.NameData.Name == itemToRemove.ItemDataFlyweight.NameData.Name);
        if (existingItem != null)
        {
            existingItem.quantity -= itemToRemove.quantity;
            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform child in m_InventoryUI)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in items)
        {
            GameObject slot = Instantiate(m_ItemSlotPrefab, m_InventoryUI);
            slot.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.ItemDataFlyweight.Icon;
            slot.transform.Find("ItemName").GetComponent<Text>().text = item.ItemDataFlyweight.NameData.Name;
            slot.transform.Find("ItemQuantity").GetComponent<Text>().text = item.quantity.ToString();
        }
    }
}
