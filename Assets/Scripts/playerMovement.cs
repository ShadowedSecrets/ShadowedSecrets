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
        
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        
        if (Input.GetMouseButtonDown(0))
        {
            abilities.UsePlague();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            abilities.UseDash(moveInput);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilities.UsePestilence();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            abilities.Echolocation();
        }


    }
    private void FixedUpdate()
    {
       if (!abilities.IsDashing())
        {
            Vector2 newPosition = rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
           
        }
    } }