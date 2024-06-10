using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerAbilities : MonoBehaviour
{
    // COOLDOWN IMAGES
    public Image plagueCooldownImage;
    public Image dashCooldownImage;
    public Image slowCooldownImage;

    // COOLDOWNS
    public float plagueCooldown = 0.4f;
    public float slowCooldown = 6f;

    // DASH VARIABLES
    public float dashCooldown = 3f;
    public float dashSpeed = 21f;
    public float dashDuration = 0.2f;

    private float plagueTimer;
    private float dashTimer;
    private float slowTimer;
    private float dashTime;

    // PESTILENCE VARIABLES
    public float slowDuration = 3f;
    public float slowRadius = 5f;
    public float slowAmount = 1f;
    public GameObject pestilenceEffect;

    private bool isDashing;
    private Vector2 dashDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        plagueTimer = 0f;
        dashTimer = 0f;
        slowTimer = 0f;

        plagueCooldownImage.fillAmount = 0;
        dashCooldownImage.fillAmount = 0;
        slowCooldownImage.fillAmount = 0;
    }

    void Update()
    {
        if (plagueTimer > 0f)
        {
            plagueTimer -= Time.deltaTime;
            plagueCooldownImage.fillAmount = plagueTimer / plagueCooldown;
        }
        else
        {
            plagueCooldownImage.fillAmount = 0;
        }

        if (dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            dashCooldownImage.fillAmount = dashTimer / dashCooldown;
        }
        else
        {
            dashCooldownImage.fillAmount = 0;
        }

        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            slowCooldownImage.fillAmount = slowTimer / slowCooldown;
        }
        else
        {
            slowCooldownImage.fillAmount = 0;
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
        if (plagueTimer <= 0f)
        {
            Debug.Log("Plague Used");
            plagueTimer = plagueCooldown;
            plagueCooldownImage.fillAmount = 1;
        }
    }

    public void UseDash(Vector2 moveInput)
    {
        if (dashTimer <= 0f && moveInput != Vector2.zero && !isDashing)
        {
            Debug.Log("Dash Used");
            isDashing = true;
            dashTime = dashDuration;
            dashDirection = moveInput.normalized;
            dashTimer = dashCooldown;
            dashCooldownImage.fillAmount = 1;
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayDashSound();
            }
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

            if(AudioManager.instance != null)
            {
                AudioManager.instance.PlayPestSound();
            }

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy found: " + enemy.name);
                    StartCoroutine(SlowEnemy(enemy.GetComponent<Enemy>()));
                }
            }
            slowTimer = slowCooldown;
            slowCooldownImage.fillAmount = 1;
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


