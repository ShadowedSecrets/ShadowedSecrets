using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillDetector : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius to detect enemy deaths
    public int requiredKills = 10; // Number of kills required to stop spawners and spawn a key
    public GameObject keyPrefab; // The key prefab to spawn
    public Transform keySpawnPoint; // The location to spawn the key
    public List<GameObject> enemySpawners; // List of enemy spawners to stop

    private int killCount = 0; // Counter for the number of enemies killed

    void Start()
    {
        // Optionally, visualize the detection radius in the editor
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = detectionRadius;
    }

    public void NotifyEnemyDeath()
    {
        killCount++;

        if (killCount >= requiredKills)
        {
            StopSpawners();
            InstantiateKey();
            Destroy(gameObject);
        }
    }

    private void StopSpawners()
    {
        foreach (var spawner in enemySpawners)
        {
            spawner.SetActive(false);
        }
    }

    private void InstantiateKey()
    {
        GameObject key = Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        KeyManager keyManager = key.GetComponent<KeyManager>();
        if (keyManager != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            keyManager.Initialize(player);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}



