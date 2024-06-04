using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAbilities : MonoBehaviour
{

    //COOLDOWNS
    public float plagueCooldown = 2f;
    public float clawCooldown = 1f;
    public float slowCooldown = 6f;

    //DASH VARIABLES
    public float dashCooldown = 3f;
    public float dashSpeed = 21f;
    public float dashDuration = 0.2f;

    private float plagueTimer;
    private float clawTimer;
    private float dashTimer;
    private float slowTimer;
    private float dashTime;

    //PESTILENCE VARIABLES
    public float slowDuration = 3f;
    public float slowRadius = 5f;
    public float slowAmount = 1f;
    public GameObject pestilenceEffect;

    

    private bool isDashing;
    private Vector2 dashDirection;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        plagueTimer = 0f;
        clawTimer = 0f;
        dashTimer = 0f;
        slowTimer = 0f;
        
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
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            rb.velocity = dashDirection * dashSpeed;

            if (dashTime <= 0)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;
            }
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
        if (dashTime <= 0f && moveInput != Vector2.zero && !isDashing)
        {
            Debug.Log("Dash Used");

            isDashing = true;
            dashTime = dashDuration;

            dashDirection = moveInput.normalized;
            dashTimer = dashCooldown;
        }
    }

    public void UsePestilence()
    {
        if (slowTimer <= 0f)
        {
            Debug.Log("PESTILENCE");
            GameObject effect = Instantiate(pestilenceEffect, transform.position, Quaternion.identity);
            effect.GetComponent<Animator>().Play("PestilenceWave");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, slowRadius);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy found: " + enemy.name);
                    StartCoroutine(SlowEnemy(enemy.GetComponent<Enemy>()));
                    //Score.score += 1;

                }
            }
            slowTimer = slowCooldown;
        }
    }

    private IEnumerator SlowEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.ModifySpeed(slowAmount);
            yield return new WaitForSeconds(slowDuration);
            enemy.ResetSpeed();
        }
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
    
}
