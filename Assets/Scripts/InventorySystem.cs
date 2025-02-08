using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int maxQuickAccessSlots = 5;  // 5 слотов в быстром доступе
    public int maxFullInventorySlots = 10; // 10 слотов в полном инвентаре

    private List<ItemData> inventoryItems = new List<ItemData>();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;

    public void AddItem(ItemData item)
    {
        if (inventoryItems.Count < maxFullInventorySlots)
        {
            inventoryItems.Add(item);
            onInventoryChanged?.Invoke(); // Обновляем UI
        }
        else
        {
            Debug.Log("Инвентарь полон!");
        }
    }

    public List<ItemData> GetQuickAccessItems()
    {
        return inventoryItems.GetRange(0, Mathf.Min(maxQuickAccessSlots, inventoryItems.Count));
    }

    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(inventoryItems);
    }
}