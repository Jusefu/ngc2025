using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public float damage;
    public float critChance;
    public float attackSpeed;
    public ElementType element;


    public enum ElementType { None, Fire, Ice, Lightning, Poison }
}

[System.Serializable]
public class RangeModifier
{
    public float damage;
    public float critChance;
    public float attackSpeed;
}
