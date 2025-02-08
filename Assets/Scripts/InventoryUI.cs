using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject quickAccessPanel; // ������ �������� �������
    public GameObject fullInventoryPanel; // ������ ���������
    public GameObject slotPrefab; // ������ �����

    private InventorySystem inventory;
    private bool isFullInventoryVisible = false;

    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        inventory.onInventoryChanged += UpdateUI; // ������������� �� ����������
        UpdateUI();
        fullInventoryPanel.SetActive(false); // �������� ��������� ��� ������
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

