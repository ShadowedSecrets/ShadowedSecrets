using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 cursorPos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;

    private float timeToDelete = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursorPos - transform.position;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        if (direction.x < 0)
        {
            Flip();
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {

        Destroy(gameObject, timeToDelete);
        
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
