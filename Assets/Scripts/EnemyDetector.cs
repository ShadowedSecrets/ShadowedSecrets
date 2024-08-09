using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillDetector : MonoBehaviour
{
    public float detectionRadius = 5f; 
    public int requiredKills = 10; 
    public GameObject keyPrefab;
    public Transform keySpawnPoint; 
    public List<GameObject> enemySpawners; 

    private int killCount = 0;

    void Start()
    {
        
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = detectionRadius;
    }

    public void NotifyEnemyDeath()
    {
        killCount++;
        Debug.Log("killing enemey");

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



