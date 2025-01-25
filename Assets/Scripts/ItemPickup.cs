using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Sprite itemIcon; // ������ ��������

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            if (inventory != null)
            {
                inventory.AddItem(itemIcon);
                Destroy(gameObject); // ������� ������� �� ����
            }
        }
    }
}
