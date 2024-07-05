using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField]
    private float maxSpeed = 2, acceleration = 50, deceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    private Vector2 idleDirection;
    private float idleTimer;
    public float idleTime = 0.5f;

    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Initialize movement direction randomly
        idleDirection = Random.insideUnitCircle.normalized;
        idleTimer = idleTime;
    }

    private void FixedUpdate()
    {
        if (MovementInput.magnitude > 0)
        {
            // Update movement
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
        else
        {
            // Handle idle movement
            if (idleTimer > 0)
            {
                // Idle movement
                currentSpeed = 1f; // Idle speed
                rb2d.velocity = idleDirection * currentSpeed;
                idleTimer -= Time.deltaTime;
            }
            else
            {
                // Change direction and reset timer
                idleDirection = Random.insideUnitCircle.normalized;
                idleTimer = idleTime;
            }

            return; // Exit FixedUpdate to avoid overwriting velocity
        }

        // Apply movement
        rb2d.velocity = oldMovementInput * currentSpeed;
    }
}