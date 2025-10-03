using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // Assign the boss prefab in the Inspector
    public Transform player; // Reference to the player's position
    public float bossSpawnDistance = 10f; // Distance from the player to spawn the boss

    void Update()
    {
        // Check if there are no enemies left
        if (AreEnemiesDefeated())
        {
            SpawnBoss();
        }
    }

    private bool AreEnemiesDefeated()
    {
        // Find all active enemies by tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }

    private void SpawnBoss()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set!");
            return;
        }

        // Calculate spawn position at a distance from the player
        Vector3 spawnPosition = player.position + (Vector3.forward * bossSpawnDistance);

        // Spawn the boss
        Instantiate(bossPrefab, spawnPosition, bossPrefab.transform.rotation);
        Debug.Log("Boss spawned at: " + spawnPosition);

        // Disable this script to prevent spawning multiple bosses
        this.enabled = false;
    }
}
