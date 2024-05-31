using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAbilities : MonoBehaviour
{
    public float plagueCooldown = 2f;
    public float clawCooldown = 1f;
    public float dashCooldown = 3f;

    private float plagueTimer;
    private float clawTimer;
    private float dashTimer;

    public float dashDistance = 2;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        plagueTimer = 0f;
        clawTimer = 0f;
        dashTimer = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plagueTimer > 0f)
        {
            plagueTimer -= Time.deltaTime;
        }
        if (clawTimer > 0f)
        {
            clawTimer -= Time.deltaTime;
        }
        if(dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }
        
    }

    public void UsePlague()
    {

        
        if(plagueTimer <= 0f)
        {
            Debug.Log("Plague Used");

            
         
            plagueTimer = plagueCooldown;

        }
    }

    public void UseClaw()
    {
        if(clawTimer <= 0f)
        {                                                                           //START OF CLAW LOGIC we need Collider[] hitEnemies, to detect the enemy layer.
            Debug.Log("Claw Used");

            clawTimer = clawCooldown;
        }
    }

    public void UseDash(Vector2 moveInput)
    {
        if (dashTimer <= 0f && moveInput != Vector2.zero)
        {
            Debug.Log("DASHED");
            Vector2 dashDirection = moveInput.normalized * dashDistance;
            rb.MovePosition(rb.position + dashDirection);
            dashTimer = dashCooldown;

        }
    }
}
