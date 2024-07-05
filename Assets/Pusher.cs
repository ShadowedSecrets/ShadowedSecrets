using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public float pushForce = 1f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pushable"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = collision.contacts[0].point - (Vector2)transform.position;
                direction = direction.normalized;
                rb.AddForce(direction * pushForce);
            }
        }
    }
}

