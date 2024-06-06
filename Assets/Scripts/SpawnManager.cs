using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> spawners;

    
    public void StartSpawners()
    {
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.StartSpawn();
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartSpawners();
        }
    }
}

