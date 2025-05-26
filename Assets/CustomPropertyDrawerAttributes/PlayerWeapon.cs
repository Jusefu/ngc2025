using UnityEngine;
using static WeaponStats;

public class PlayerWeapon : MonoBehaviour
{
    public bool useProjectile = false;

    [ConditionalField("useProjectile")]
    public GameObject projectilePrefab;

    [ConditionalField("useProjectile")]
    public float projectileSpeed = 10f;

    public bool applyElementalDamage = false;

    [ConditionalField("applyElementalDamage")]
    public ElementType elementType;
}