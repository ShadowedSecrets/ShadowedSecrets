using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public Rigidbody2D rb; // refrence to rigid body in the player inspector.
    public float moveSpeed; // sets moving speed for player.
    float inputX , inputY; // sets the variables for inputX and inputY
    
    void Start()
    {
        // gets the rigid body for player 
        rb = gameObject.GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        // gets the input for both inputs (X and Y).
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        
    }
    private void FixedUpdate()
    {
        // makes the player move by creating a new vector2 with both inputX and input.
        rb.velocity = new Vector2(inputX, inputY).normalized * moveSpeed;
    }
}
