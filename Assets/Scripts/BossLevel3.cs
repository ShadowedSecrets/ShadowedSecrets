using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel3 : MonoBehaviour, IBoss
{
    public enum BossPhase { SPAWN, CHASE }
    private BossPhase currentPhase;

    public float speed = 2f;
    public Transform player;
    public GameObject projectilePrefab;
    public GameObject[] enemyPrefabs; // Array to store different enemy types
    public GameObject ladder;
    public float abilityInterval = 5f;
    public int numberOfProjectiles = 12;
    public float projectileSpeed = 5f;
    public int maxHealth = 15;
    public int numberOfCircularShots = 4;
    public float circularShotInterval = 1f;
    public int numberOfEnemiesToSpawn = 20;
    public int coneProjectiles = 5;
    public float coneAngle = 10f;
    public float spawnPhaseDuration = 15f; // Time the boss remains in SPAWN phase
    public float chasePhaseDuration = 10f; // Time the boss remains in CHASE phase

    private float abilityTimer;
    private int currentHealth;
    private Rigidbody2D rb;
    private bool isActive = false;
    private bool isShooting = false;
    public BossHealthUI3 bossHealthUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        abilityTimer = abilityInterval;
        currentHealth = maxHealth;

        if (bossHealthUI == null)
        {
            Debug.LogError("BossHealthUI is not assigned in the inspector!");
        }

        if (ladder == null)
        {
            Debug.LogError("Ladder is not assigned in the inspector!");
        }
        else
        {
            ladder.SetActive(false); // Ensure ladder is initially inactive
        }

        currentPhase = BossPhase.SPAWN;
        StartCoroutine(HandleBossPhases());
    }

    void Update()
    {
        if (!isActive || isShooting) return;

        if (currentPhase == BossPhase.CHASE)
        {
            MoveTowardsPlayer();

            abilityTimer -= Time.deltaTime;
            if (abilityTimer <= 0)
            {
                FireConeProjectiles();
                abilityTimer = abilityInterval;
            }
        }
    }

    private IEnumerator HandleBossPhases()
    {
        while (true)
        {
            if (currentPhase == BossPhase.SPAWN)
            {
                yield return StartCoroutine(SpawnEnemies());
                yield return new WaitForSeconds(spawnPhaseDuration);
                currentPhase = BossPhase.CHASE;
            }
            else if (currentPhase == BossPhase.CHASE)
            {
                yield return new WaitForSeconds(chasePhaseDuration);
                currentPhase = BossPhase.SPAWN;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        isShooting = true;
        rb.velocity = Vector2.zero;

        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            float angle = i * (360f / numberOfEnemiesToSpawn);
            Vector3 spawnPosition = transform.position + (Quaternion.Euler(0, 0, angle) * Vector3.right * 3f);
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        yield return null; // End the coroutine and allow the phase transition to occur after the set duration
        isShooting = false;
    }

    private void FireConeProjectiles()
    {
        isShooting = true;
        rb.velocity = Vector2.zero;

        
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        
        float startAngle = baseAngle - (coneAngle * (coneProjectiles - 1)) / 2;

        for (int i = 0; i < coneProjectiles; i++)
        {
            float angle = startAngle + i * coneAngle;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            FireProjectile(direction);
        }

        isShooting = false;
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentPhase == BossPhase.SPAWN) return;

        currentHealth -= damageAmount;
        Debug.Log("Boss took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss died");
        Destroy(gameObject);
        bossHealthUI.Hide();
        ladder.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("Boss hit by projectile.");
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.damage);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Boss collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Boss collided with wall.");
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                Debug.LogError("Rigidbody2D is null on Boss.");
            }
        }
        else
        {
            Debug.LogWarning("Collision with non-wall object detected.");
        }
    }

    public void Activate()
    {
        Debug.Log("Boss activated.");
        isActive = true;
        bossHealthUI.Initialize(this);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}