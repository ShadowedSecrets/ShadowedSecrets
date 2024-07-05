using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float chaseRange = 2.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int health = 3; // Enemy health, set to 3 for 3 hits
    [SerializeField] private int damage = 1; // Damage to player

    private bool isChasing = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetTarget();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseRange)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }

            if (isChasing)
            {
                ChasePlayer();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            rb.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage, health now: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Enemy collided with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player");
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject); // Optional: Destroy enemy after hitting the player
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on Player!");
            }
        }
        
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit enemy");
            TakeDamage(1);
            Destroy(other.gameObject); // Destroy the projectile
        }
    }

    private void GetTarget()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }
}
