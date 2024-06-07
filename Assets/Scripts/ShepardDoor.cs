using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public  List<EnemySpawner> enemySpawners3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player triggered the door.");
            StopSpawners();
            Destroy(gameObject);
        }
    }

    private void StopSpawners()
    {
        foreach (EnemySpawner spawner in enemySpawners3)
        {
            spawner.StopSpawn();
        }
    }
}
