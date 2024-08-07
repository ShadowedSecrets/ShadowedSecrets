using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 cursorPos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public int damage = 1;
    private float timeToDelete = 2f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorPos - (Vector2)transform.position;

        rb.velocity = direction.normalized * force;

        if (direction.x < 0)
        {
            Flip();
        }

        Destroy(gameObject, timeToDelete);
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale; 
        localScale.x *= -1;
        transform.localScale = localScale;
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
            Boss boss = collision.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
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

