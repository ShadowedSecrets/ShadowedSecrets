using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;
    public SpriteRenderer player;

    public GameObject PlayerRs;
    public Transform respawnPoint;

    public playerMovement movement;

    private Coroutine damageCoroutine;
    private float timeInLight = 0f;
    private bool isInLight = false;

    public ParticleSystem collsionParticleSystem;
    public SpriteRenderer sR;
    public bool once = true;

    private Vector3 lastCheckpointPosition;

    private Animator animator;

    void Start()
    {
        health = maxHealth;

        
        lastCheckpointPosition = PlayerRs.transform.position;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInLight)
        {
            timeInLight += Time.deltaTime;

            if (timeInLight >= 0.2f && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime(0.5f, 1));
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        animator.SetTrigger("Damage");

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayPlayerHitSound();
        }

        if (health <= 0)
        {
            Respawn();
        }
    }

    public void AddHealth()
    {
        if (health < maxHealth)
        {
            health += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //finding the last checkpoint
        if (other.CompareTag("CheckPoint"))
        {
            lastCheckpointPosition = other.transform.position;
        }

        if (other.CompareTag("Light"))
        {
            isInLight = true;
            timeInLight = 0f;
        }

        if (other.CompareTag("Potion"))
        {
            AddHealth();
            var em = collsionParticleSystem.emission;
            var dur = collsionParticleSystem.duration;
            em.enabled = true;
            collsionParticleSystem.Play();
            once = false;

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayPotionSound();
            }

            Destroy(other.gameObject);
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

    private void Respawn()
    {
        health = maxHealth;
        PlayerRs.transform.position = lastCheckpointPosition;

        player.enabled = true;
        movement.enabled = true;
    }
}
