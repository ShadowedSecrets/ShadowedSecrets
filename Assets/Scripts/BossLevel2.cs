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

    private float abilityTimer;
    private bool isCircularAbility;
    private int currentHealth;
    private Rigidbody2D rb;
    private bool isActive = false;
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
        if (!isActive) return;

        MoveTowardsPlayer();

        abilityTimer -= Time.deltaTime;
        if (abilityTimer <= 0)
        {
            if (isCircularAbility)
            {
                StartCoroutine(FireCircularProjectiles());
            }
            else
            {
                StartCoroutine(FireTargetedProjectiles());
            }

            isCircularAbility = !isCircularAbility;
            abilityTimer = abilityInterval;
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

    private IEnumerator FireCircularProjectiles()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * (360f / numberOfProjectiles);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            FireProjectile(direction);
        }
        yield return null;
    }

    private IEnumerator FireTargetedProjectiles()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            FireProjectile((player.position - transform.position).normalized);
            yield return new WaitForSeconds(0.1f);
        }
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

