using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns = 2.0f; // Time between each enemy spawn within a wave
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenWaves = 10.0f;
    private bool canSpawn = false;
    private Coroutine spawnerCoroutine;

    [SerializeField] private SummoningCircle summoningCircle;
    [SerializeField] private float prewaveDuration = 2.0f; // Duration of the prewave effect

    private IEnumerator Spawner()
    {
        while (true)
        {
            if (canSpawn)
            {
                // Trigger the prewave effect
                summoningCircle.StartPrewave(prewaveDuration);
                yield return new WaitForSeconds(prewaveDuration);

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void StartSpawn()
    {
        if (!canSpawn)
        {
            canSpawn = true;
            spawnerCoroutine = StartCoroutine(Spawner());
        }
    }

    public void StopSpawn()
    {
        if (canSpawn)
        {
            canSpawn = false;
            if (spawnerCoroutine != null)
            {
                StopCoroutine(spawnerCoroutine);

                spawnerCoroutine = null;
                Destroy(gameObject);
            }
        }
    }
}
