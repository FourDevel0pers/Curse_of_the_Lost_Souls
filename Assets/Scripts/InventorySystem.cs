using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<Sprite> inventoryItems = new List<Sprite>(); // Список предметов (иконок)
    public GameObject quickAccessPanel; // Панель быстрого доступа (5 слотов)
    public GameObject fullInventoryPanel; // Панель полного инвентаря (10 слотов)
    public GameObject slotPrefab; // Префаб для отображения слотов
    public int maxQuickAccessSlots = 5;
    public int maxFullInventorySlots = 10;

    private bool isFullInventoryVisible = false;

    void Start()
    {
        UpdateUI();
        fullInventoryPanel.SetActive(false); // Полный инвентарь скрыт
    }

    void Update()
    {
        // Открытие/закрытие полного инвентаря при нажатии Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFullInventoryVisible = !isFullInventoryVisible;
            fullInventoryPanel.SetActive(isFullInventoryVisible);
        }
    }

    public void AddItem(Sprite itemIcon)
    {
        if (inventoryItems.Count < maxFullInventorySlots)
        {
            inventoryItems.Add(itemIcon);
            UpdateUI();
        }
        else
        {
            Debug.Log("Инвентарь полон!");
        }
    }

    void UpdateUI()
    {
        // Обновление панели быстрого доступа
        foreach (Transform child in quickAccessPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < Mathf.Min(maxQuickAccessSlots, inventoryItems.Count); i++)
        {
            CreateSlot(quickAccessPanel, inventoryItems[i]);
        }

        // Обновление панели полного инвентаря
        foreach (Transform child in fullInventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            CreateSlot(fullInventoryPanel, inventoryItems[i]);
        }
    }

    void CreateSlot(GameObject parent, Sprite itemIcon)
    {
        GameObject slot = Instantiate(slotPrefab, parent.transform);
        slot.GetComponent<Image>().sprite = itemIcon;
    }
}