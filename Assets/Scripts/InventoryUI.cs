using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject quickAccessPanel; // Панель быстрого доступа
    public GameObject fullInventoryPanel; // Полный инвентарь
    public GameObject slotPrefab; // Префаб слота

    private InventorySystem inventory;
    private bool isFullInventoryVisible = false;

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        inventory.onInventoryChanged += UpdateUI; // Подписываемся на обновление
        UpdateUI();
        fullInventoryPanel.SetActive(false); // Скрываем инвентарь при старте
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFullInventoryVisible = !isFullInventoryVisible;
            fullInventoryPanel.SetActive(isFullInventoryVisible);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        UpdatePanel(quickAccessPanel, inventory.GetQuickAccessItems());
        UpdatePanel(fullInventoryPanel, inventory.GetAllItems());
    }

    void UpdatePanel(GameObject panel, List<ItemData> items)
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData item in items)
        {
            GameObject slot = Instantiate(slotPrefab, panel.transform);
            Image itemImage = slot.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.itemIcon;
            }
        }
    }
}

