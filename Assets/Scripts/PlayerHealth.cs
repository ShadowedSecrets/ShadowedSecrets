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


    private Vector3 checkpoint1Position;
    private Vector3 checkpoint2Position;
    private int lastCheckpoint = 0;

    private Animator animator;

    void Start()
    {
        health = maxHealth;

        checkpoint1Position = new Vector3(18, 8.25f, 0);
        checkpoint2Position = new Vector3(56, 9, 0);

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
            if (lastCheckpoint == 3)
            {
                player.enabled = false;
                movement.enabled = false;
                Destroy(gameObject, 0.1f);
                SceneManager.LoadScene("EndScene");
            }
            else
            {
                Respawn();
            }



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

        if (other.CompareTag("CheckPoint1"))
        {
            lastCheckpoint = 1;
        }
        else if (other.CompareTag("CheckPoint2"))
        {
            lastCheckpoint = 2;
        }
        else if (other.CompareTag("CheckPoint3"))
        {
            lastCheckpoint = 3;
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
        if (lastCheckpoint == 1)
        {
            PlayerRs.transform.position = checkpoint1Position;
        }
        else if (lastCheckpoint == 2)
        {
            PlayerRs.transform.position = checkpoint2Position;
        }
        player.enabled = true;
        movement.enabled = true;
    }




}
