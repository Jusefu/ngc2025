using UnityEngine;



public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnRadius = 5f;

    
    [ContextMenu("Spawn Enemies Now")]
    void SpawnEnemiesNow()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 randomPos = transform.position +
                             Random.insideUnitSphere * spawnRadius;
            randomPos.y = 0; // Keep on ground
            Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        }

        Debug.Log($"Spawned {maxEnemies} enemies around {gameObject.name}");
    }

    [ContextMenuItem("Reset", "ResetHealth")]
    [ContextMenuItem("Set Max", "SetMaxHealth")]
    public int health = 50;

    void ResetHealth()
    {
        health = 50;
    }

    void SetMaxHealth()
    {
        health = 100;
    }

}