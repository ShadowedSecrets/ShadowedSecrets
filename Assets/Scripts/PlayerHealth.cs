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

    private Coroutine damageCoroutine;
    private float timeInLight = 0f;
    private bool isInLight = false;



  
    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (isInLight)
        {
            timeInLight += Time.deltaTime;

            if (timeInLight >= 1f && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime(1f, 1));
            }
        }
    }


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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            isInLight = true;
            timeInLight = 0f;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            isInLight = false;
            timeInLight = 0f;

            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    

    private IEnumerator DamageOverTime(float interval, int damageAmount)
    {
        while (true)
        {
            TakeDamage(damageAmount);
            yield return new WaitForSeconds(interval);
        }
    }


}
