using UnityEngine;

public class AICharacterStats : MonoBehaviour
{
    [ColoredHeader("Player Stats", 15f, 0.3f, 0.6f, 0.3f)]
    [SerializeField] private float health;
    [SerializeField] private float armor;

    [ColoredHeader("Combat Settings", 15f, 0.6f, 0.3f, 0.3f)]
    [SerializeField] private float attackPower;
    [SerializeField] private float critChance;
}
