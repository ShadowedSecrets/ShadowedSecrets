using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBAT : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;
    public float rotateSpeed = 0.0025f;
    public float zigzagAmplitude = 1.0f; // Amplitude of the zigzag
    public float zigzagFrequency = 1.0f; // Frequency of the zigzag
    public float dashSpeed = 5.0f; // Speed during the dash
    public float dashDistance = 2.0f; // Distance to player to start dashing
    public float dashDuration = 1.0f; // Duration of the dash
    private Rigidbody2D rb;
    public int damage = 1;
    public int maxHealth = 3; // Maximum health for the enemy
    private int currentHealth;
    private float originalSpeed;
    private bool isDead = false;
    private bool isDashing = false;
    private Vector2 dashTarget;
    private Coroutine dashCoroutine;

    public List<LootItem> lootTable = new List<LootItem>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        currentHealth = maxHealth; // Initialize health
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }

       

        if (isDashing)
        {
            DashTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            if (Vector2.Distance(transform.position, target.position) <= dashDistance)
            {
                isDashing = true;
                dashTarget = target.position;
                speed = dashSpeed;
                dashCoroutine = StartCoroutine(StopDashAfterTime(dashDuration));
               
            }
        }
    }



    private void DashTowardsTarget()
    {
        Vector2 dashDirection = (dashTarget - (Vector2)transform.position).normalized;
        rb.velocity = dashDirection * speed;
    }

    private void rotateTowardsTarget()
    {
        Vector2 targetDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private IEnumerator StopDashAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isDashing = false;
        speed = originalSpeed;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Die(); // Enemy dies after hitting the player
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile") && !isDead)
        {
            TakeDamage(1);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayEnemyHitSound();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Prevent taking damage if already dead

        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        Debug.Log("Enemy died");
        isDead = true;
        Score.score += 1;

        // Notify the kill detector
        EnemyKillDetector killDetector = FindObjectOfType<EnemyKillDetector>();
        if (killDetector != null)
        {
            float distanceToDetector = Vector2.Distance(transform.position, killDetector.transform.position);
            if (distanceToDetector <= killDetector.detectionRadius)
            {
                killDetector.NotifyEnemyDeath();
            }
        }

        foreach(LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            break;
        }

        Destroy(gameObject);
    }
    void InstantiateLoot(GameObject loot)
    {
        GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
    }

    public void ModifySpeed(float speedFactor)
    {
        speed = originalSpeed * speedFactor;
        
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
        
    }

    public bool IsDead()
    {
        return isDead;
    }
}
