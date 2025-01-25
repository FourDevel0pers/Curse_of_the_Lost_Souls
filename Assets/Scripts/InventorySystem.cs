using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<Sprite> inventoryItems = new List<Sprite>(); // ������ ��������� (������)
    public GameObject quickAccessPanel; // ������ �������� ������� (5 ������)
    public GameObject fullInventoryPanel; // ������ ������� ��������� (10 ������)
    public GameObject slotPrefab; // ������ ��� ����������� ������
    public int maxQuickAccessSlots = 5;
    public int maxFullInventorySlots = 10;

    private bool isFullInventoryVisible = false;

    void Start()
    {
        UpdateUI();
        fullInventoryPanel.SetActive(false); // ������ ��������� �����
    }

    void Update()
    {
        // ��������/�������� ������� ��������� ��� ������� Tab
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
            Debug.Log("��������� �����!");
        }
    }

    void UpdateUI()
    {
        // ���������� ������ �������� �������
        foreach (Transform child in quickAccessPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < Mathf.Min(maxQuickAccessSlots, inventoryItems.Count); i++)
        {
            CreateSlot(quickAccessPanel, inventoryItems[i]);
        }

        // ���������� ������ ������� ���������
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