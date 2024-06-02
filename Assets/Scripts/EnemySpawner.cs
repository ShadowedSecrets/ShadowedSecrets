using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool canSpawn = true;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);    

        while (canSpawn)
        {
            yield return wait;

            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
