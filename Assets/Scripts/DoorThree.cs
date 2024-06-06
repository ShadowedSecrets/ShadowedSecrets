using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorThree : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> enemySpawners2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("KeyTwo"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            foreach (EnemySpawner spawner in enemySpawners2)
            {
                if (spawner != null)
                {
                    spawner.StartSpawn();
                }
            }
        }
    }
}