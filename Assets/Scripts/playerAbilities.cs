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
    public GameObject plagueBolt;
    public Transform projectileSpawn;
    public float projectileSpeed = 5f;


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
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 direction = (worldPosition - projectileSpawn.position).normalized;
            GameObject projectile = Instantiate(plagueBolt, projectileSpawn.position, projectileSpawn.rotation);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(direction.x, direction.y) * projectileSpeed;
            }

         
            plagueTimer = plagueCooldown;

        }
    }

    public void UseClaw()
    {
        if(clawTimer <= 0f)
        {
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
