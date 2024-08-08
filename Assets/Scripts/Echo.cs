using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{

    private Rigidbody2D rb;
    public int damage = 1;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 2f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            IBoss boss = collision.GetComponent<IBoss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit");
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
