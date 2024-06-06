using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    public int damage = 1;
    public int maxHealth = 3; // Maximum health for the enemy
    private int currentHealth;
    private float originalSpeed;
    private bool isDead = false;

    // Start is called before the first frame update
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
        else
        {
            rotateTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Die(); // Enemy dies after hitting the player
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1); // Take damage from projectile
            
        }
    }

    public void TakeDamage(int damageAmount)
    {
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

        Destroy(gameObject);
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






