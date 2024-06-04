using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float chaseRange = 2.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int health = 3; // Enemy health, set to 3 for 3 hits

    private bool isChasing = false;

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

    private void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            
            TakeDamage(1);
            Destroy(other.gameObject); 
        }
    }
}


