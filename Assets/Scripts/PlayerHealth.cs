using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 4;
    public SpriteRenderer player;
    public playerMovement movement;



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
            player.enabled = false;
            movement.enabled = false;
            Destroy(gameObject, 0.1f);
            SceneManager.LoadScene("EndScene");

        }
    }


}
