using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public Rigidbody2D rb; // refrence to rigid body in the player inspector.
    public float moveSpeed = 5f; // sets moving speed for player.
    private Vector2 moveInput;


    private playerAbilities abilities;
    private bool isDashing = false;

    void Start()
    {
        // gets the rigid body for player 
        rb = gameObject.GetComponent<Rigidbody2D>();
        //refrence for abilities
        abilities = GetComponent<playerAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        // gets the input for both inputs (X and Y).
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        //Key presses
        if (Input.GetMouseButtonDown(0))
        {
            abilities.UsePlague();
        }
        if (Input.GetMouseButtonDown(1))
        {
            abilities.UseClaw();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            abilities.UseDash(moveInput);
            isDashing = true;
        }


    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            isDashing = false;
        }
        else
        {
            Vector2 newPosition = rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
    }
    } }