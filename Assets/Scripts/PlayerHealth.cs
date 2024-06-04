using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 4;
   


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    //Player health counter
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
           
        }
    }

    public void Die()
    {
        
        Destroy(gameObject);
    }
}
