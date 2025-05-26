using UnityEditor.Rendering;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [RangeWithColor(0, 100, 50, "#88880088")]
    public float health = 20f;
    [RangeWithColor(0, 100, 80, "#88000088")]
    public float AttackPower = 60f;
}
