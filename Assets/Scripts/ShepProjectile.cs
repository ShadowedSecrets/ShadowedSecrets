using UnityEngine;

public class ShepProjectile : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the projectile
    public float lifetime = 2f; // Time before the projectile destroys itself
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            BounceOff(collision);
        }

    }
    private void BounceOff(Collider2D other)
    {
        Vector2 collisionNormal = ((Vector2)transform.position - other.ClosestPoint(transform.position)).normalized;
        Vector2 newVelocity = Vector2.Reflect(rb.velocity, collisionNormal);
        rb.velocity = newVelocity;
    }
}

