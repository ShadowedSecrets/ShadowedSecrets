using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth = 4;
    public Sprite emptyHealth;
    public Sprite fullHealth;
    public Image[] Hearts;
    public PlayerHealth playerHealth;


    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        TakeDamage();
    }
    // Player health display
    void TakeDamage()
    {
        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < health)
            {
                Hearts[i].sprite = fullHealth;
            }
            else
            {
                Hearts[i].sprite = emptyHealth;
            }
            if (i < maxHealth)
            {
                Hearts[i].enabled = true;
            }
            else
            {
                Hearts[i].enabled = false;
            }
        }
    }
}