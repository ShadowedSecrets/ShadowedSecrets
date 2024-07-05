using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{
    public GameObject blueCrystal;
    public GameObject redCrystal;
    public GameObject yellowCrystal;
    public GameObject reward;
    public GameObject reward2;
    [SerializeField] private List<EnemySpawner> enemySpawners;

    [SerializeField] public Transform rewardDrop;




    private int destructionIndex = 0;

    void Start()
    {
        
        RespawnCrystals();
    }

    public void DestroyCrystal(string color)
    {
        if (destructionIndex == 0 && color == "Blue" ||
            destructionIndex == 1 && color == "Red" ||
            destructionIndex == 2 && color == "Yellow")
        {
            destructionIndex++;

            if (destructionIndex == 3)
            {
                RewardPlayer();

                foreach (EnemySpawner spawner in enemySpawners)
                {
                    Destroy(spawner.gameObject);
                }
                
            }
        }
        else
        {
            RespawnCrystals();
        }
    }

    private void RewardPlayer()
    {
        Instantiate(reward, rewardDrop.position, Quaternion.identity);
        Instantiate(reward2, rewardDrop.position, Quaternion.identity);

        destructionIndex = 0;
    }

    private void RespawnCrystals()
    {
        blueCrystal.SetActive(true);
        redCrystal.SetActive(true);
        yellowCrystal.SetActive(true);
        destructionIndex = 0;
    }
}

