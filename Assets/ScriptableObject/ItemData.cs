using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon;
    public int value = 10;
    public string description = "Item description here";

    [TextArea(3, 10)]
    public string loreText;

    public ItemType itemType;
    public enum ItemType { Weapon, Armor, Consumable, Quest }

    // You can also add methods to ScriptableObjects!
    public string GetTooltip()
    {
        return $"{itemName}\nValue: {value} gold\n{description}";
    }
}