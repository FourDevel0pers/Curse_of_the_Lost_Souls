using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itemIcon;

    public ItemData(string name, Sprite icon)
    {
        itemName = name;
        itemIcon = icon;
    }
}