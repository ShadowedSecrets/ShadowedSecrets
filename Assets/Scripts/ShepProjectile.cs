using UnityEngine;

public class ShepProjectile : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the projectile
    public float lifetime = 2f; // Time before the projectile destroys itself

    void Start()
    {
       
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
       
    }
}

