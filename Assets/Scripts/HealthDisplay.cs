using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;
    public Slider healthSlider;
    public PlayerHealth playerHealth;


    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
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
        healthSlider.value = health;
    }
}