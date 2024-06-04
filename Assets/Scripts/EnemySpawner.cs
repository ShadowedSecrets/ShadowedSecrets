using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private float spawnRateVariation = 0.5f;
    [SerializeField] private GameObject enemyPrefab;
    private bool canSpawn = false;

    private void Start()
    {
        
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);    

        while (true)
        {
            if (canSpawn)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
            float waitTime = spawnRate + Random.Range(-spawnRateVariation, spawnRateVariation);
            yield return new WaitForSeconds (waitTime);

            
        }
    }

    public void StartSpawn()
    {
        if(!canSpawn)
        {
            canSpawn = true;
            StartCoroutine(Spawner());
        }
    }
}
