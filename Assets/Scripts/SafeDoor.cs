using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDoor : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> enemySpawners;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayDoorOpenSound();
            }
            GetComponent<ParticleSystem>().Play();
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke(nameof(DestroyObj), 0.1f);
            foreach (EnemySpawner spawner in enemySpawners)
            {
                if (spawner != null)
                {
                    spawner.StartSpawn();
                }
            }
        }
    }
    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
