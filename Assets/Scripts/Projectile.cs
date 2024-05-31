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
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursorPos - transform.position;
        Vector3 rotation = transform.position - cursorPos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        
    }

    // Update is called once per frame
    void Update()
    {

        Destroy(gameObject, timeToDelete);
        
    }
}
