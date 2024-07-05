using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Adjust speed as needed
    public float idleTime = 2f; // Time to idle in one direction before changing

    private Vector3 moveDirection;
    private float idleTimer;


    public void MoveIdle(GameObject gameObject)
    {

        // Initialize movement direction randomly
        moveDirection = Random.insideUnitSphere.normalized;
        moveDirection.y = 0; // Ensure movement stays in the horizontal plane
        idleTimer = idleTime;

        if (idleTimer > 0)
        {
            // Idle movement
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            idleTimer -= Time.deltaTime;
        }
        else
        {
            // Change direction and reset timer
            moveDirection = Random.insideUnitSphere.normalized;
            moveDirection.y = 0;
            idleTimer = idleTime;
        }
    }
}
