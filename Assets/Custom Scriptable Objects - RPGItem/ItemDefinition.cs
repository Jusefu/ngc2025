using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "RPG/Item")]
public class ItemDefinition : ScriptableObject
{
    public string itemName = "New Item";
    public string description;
    public Sprite icon;

    public ItemRarity rarity = ItemRarity.Common;
    public enum ItemRarity { Common, Uncommon, Rare, Epic, Legendary }

    public ItemType itemType;
    public enum ItemType { Weapon, Armor, Potion, Material, Quest }

    public int goldValue = 10;
    public int requiredLevel = 1;

    // Type-specific properties
    public bool isStackable;
    public int maxStackSize = 20;

    // Weapon-specific properties
    public int damageAmount;
    public float attackSpeed;
    public float critChance;

    // Armor-specific properties
    public int armorValue;
    public float movementPenalty;

    // Potion-specific properties
    public int healthRestored;
    public int manaRestored;
    public float duration;

    // Editor-only validation
    public bool hasValidIcon => icon != null;
}