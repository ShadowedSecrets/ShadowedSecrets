using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTwo : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> enemySpawners;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("KeyTwo"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            foreach (EnemySpawner spawner in enemySpawners)
            {
                if (spawner != null)
                {
                    spawner.StopSpawn();
                }
            }
        }
    }
}

