using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Transform target; 
    public float speed = 2.0f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    private float originalSpeed;
    public int damage = 1;

    // Start is called before the first frame update
    private void Start()
    {
        //sets the rigid body for the enemy 
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
    }
    private void Update()
    {   if (!target)
        {
            GetTarget();
        }
        else
        {
            rotateTowardsTarget();
        }
        
    }
    private void FixedUpdate()
    {// move forward 
        rb.velocity = transform.up * speed;
    }
    private void rotateTowardsTarget()
    { // rotates so the enemy is facing the player at all times
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {// finds the target with the tag "Player"
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Decreases player health by 1
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
            target = null;
        }
        else if(other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            Score.score += 1; // Score counter
        }
    }
    public void ModifySpeed(float speedFactor)
    {
        speed = originalSpeed * speedFactor;
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
}
