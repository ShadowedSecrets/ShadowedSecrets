using UnityEngine;

public class Crystals : MonoBehaviour
{
    public GameObject blueCrystal;
    public GameObject redCrystal;
    public GameObject yellowCrystal;
    public GameObject reward;

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
            }
        }
        else
        {
            
            RespawnCrystals();
        }
    }

    private void RewardPlayer()
    {
        Instantiate(reward, transform.position, Quaternion.identity);
        
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

