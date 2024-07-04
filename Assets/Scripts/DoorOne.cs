using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorOne : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> enemySpawners;
  
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("KeyOne"))
        {
            
            
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayDoorOpenSound();
            }
            
            GetComponent<ParticleSystem>().Play();
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other.gameObject);
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

