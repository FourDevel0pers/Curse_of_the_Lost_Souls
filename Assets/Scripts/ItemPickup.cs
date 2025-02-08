using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            if (inventory != null)
            {
                ItemData newItem = new ItemData(itemName, itemIcon);
                inventory.AddItem(newItem);
                Destroy(gameObject);
            }
        }
    }
}
