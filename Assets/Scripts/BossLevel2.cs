using System.Collections;
using UnityEngine;

public class BossLevel2 : MonoBehaviour, IBoss
{
    public float speed = 2f;
    public Transform player;
    public GameObject projectilePrefab;
    public GameObject ladder;
    public float abilityInterval = 5f;
    public int numberOfProjectiles = 12;
    public float projectileSpeed = 5f;
    public int maxHealth = 15;
    public int numberOfCircularShots = 4; // Number of times to shoot circular projectiles
    public float circularShotInterval = 1f; // Interval between circular shots

    private float abilityTimer;
    private int currentHealth;
    private Rigidbody2D rb;
    private bool isActive = false;
    private bool isShooting = false; // Flag to indicate if the boss is currently shooting
    public BossHealthUI2 bossHealthUI;

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
    }

    void Update()
    {
        if (!isActive || isShooting) return; // Stop movement if not active or currently shooting

        MoveTowardsPlayer();

        abilityTimer -= Time.deltaTime;
        if (abilityTimer <= 0)
        {
            StartCoroutine(FireCircularProjectilesCycle());
            abilityTimer = abilityInterval; // Reset the ability timer after starting the shooting cycle
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

    private IEnumerator FireCircularProjectilesCycle()
    {
        isShooting = true; // Set the shooting flag to true to stop movement
        rb.velocity = Vector2.zero; // Stop moving

        for (int i = 0; i < numberOfCircularShots; i++)
        {
            for (int j = 0; j < numberOfProjectiles; j++)
            {
                float angle = j * (360f / numberOfProjectiles);
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
                FireProjectile(direction);
            }
            yield return new WaitForSeconds(circularShotInterval);
        }

        isShooting = false; // Reset the shooting flag after finishing the cycle
        yield return null;
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
    }

    public void TakeDamage(int damageAmount)
    {
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
