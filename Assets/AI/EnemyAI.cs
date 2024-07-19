using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBATai : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehavour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 0.5f;

    // Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField]
    private bool isBat;

    [SerializeField]
    private bool isGhost;

    [SerializeField]
    private bool isZombie;

    bool following = false;

    private Rigidbody2D rb;
    public int damage = 1;
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    private bool isDead = false;
    public List<LootItem> lootTable = new List<LootItem>();

    // Freeze variables
    private bool isFrozen = false;
    private float freezeDuration;
    private Coroutine currentCoroutine;

    // Knockback variables 
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;

    // Darting variables
    [SerializeField]
    private float dartSpeed = 5f;
    [SerializeField]
    private float normalSpeed = 2f;
    [SerializeField]
    private float dartDuration = 0.5f;
    [SerializeField]
    private float dartCooldown = 2f;
    private bool isDarting = false;

    /*// Bobbing variables
    [SerializeField]
    private float bobbingAmplitude = 0.5f;
    [SerializeField]
    private float bobbingFrequency = 1f;
   // private Coroutine bobbingCoroutine;*/

    private void Start()
    {
        // Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);

        // Ensure Rigidbody2D is assigned
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }

        /*// Start bobbing movement if enemy is a ghost
        if (isGhost)
        {
            bobbingCoroutine = StartCoroutine(BobbingMovement());
        }*/
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        if (isFrozen)
        {
            // Ensure the enemy stops moving immediately
            movementInput = Vector2.zero;
            OnMovementInput?.Invoke(movementInput);
            return; // Skip further updates if frozen
        }

        // Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                currentCoroutine = StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            aiData.currentTarget = aiData.targets[0];
        }
        else
        {
            // Do nothing if no target is available
        }
        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        while (aiData.currentTarget != null && !isFrozen)
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                if (!isDarting && isBat)
                {
                    isDarting = true;
                    StartCoroutine(DartAtPlayer());
                    yield return new WaitForSeconds(dartCooldown);
                    isDarting = false;
                }

                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
            }
        }
        following = false;
    }

    private IEnumerator DartAtPlayer()
    {
        float elapsedTime = 0f;
        Vector2 directionToPlayer = (aiData.currentTarget.position - transform.position).normalized;

        while (elapsedTime < dartDuration)
        {
            movementInput = directionToPlayer * dartSpeed;
            OnMovementInput?.Invoke(movementInput);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movementInput = directionToPlayer * normalSpeed;
        OnMovementInput?.Invoke(movementInput);
    }

    // Coroutine to handle bobbing movement
   /* private IEnumerator BobbingMovement()
    {
        float elapsedTime = 0f;
        
        while (true)
        {
            if (!isFrozen && aiData.currentTarget != null)
            {

                // Calculate bobbing ofest using a sine wave
                float bobbingOffset = Mathf.Sin(elapsedTime * bobbingFrequency) * bobbingAmplitude;

                // update the base movement input towards the player 
                Vector2 directionToPlayer = (aiData.currentTarget.position - transform.position).normalized;
                Vector2 baseMovementInput = directionToPlayer * normalSpeed;

                // combine base movement with bibbing effect 
                Vector2 combinedMovement = baseMovementInput;
                combinedMovement.y += bobbingOffset;

                //move ghost enemy 
                rb.MovePosition(rb.position + combinedMovement * Time.deltaTime); 
                elapsedTime += Time.deltaTime;
            }
           
            yield return null;
        }
        
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1, other);

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayEnemyHitSound();
            }
        }
    }

    public void TakeDamage(int damageAmount, Collider2D other)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockBack(0.3f);
        }
    }

    private void Die()
    {
        if (isDead) return;

        Debug.Log("Enemy died");
        isDead = true;
        Score.score += 1;

        EnemyKillDetector killDetector = FindObjectOfType<EnemyKillDetector>();
        if (killDetector != null)
        {
            float distanceToDetector = Vector2.Distance(transform.position, killDetector.transform.position);
            if (distanceToDetector <= killDetector.detectionRadius)
            {
                killDetector.NotifyEnemyDeath();
            }
        }

        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            break;
        }

        Destroy(gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void ApplyFreeze(float duration)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            freezeDuration = duration;
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            StartCoroutine(FreezeEnemy());
        }
    }

    private IEnumerator FreezeEnemy()
    {
        // Stop all movement
        movementInput = Vector2.zero;
        OnMovementInput?.Invoke(movementInput);

        yield return new WaitForSeconds(freezeDuration);

        isFrozen = false;
        if (aiData.currentTarget != null)
        {
            currentCoroutine = StartCoroutine(ChaseAndAttack());
        }
    }

    public void ApplyKnockBack(float duration)
    {
        freezeDuration = duration;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        StartCoroutine(EnemyStun());
    }

    public IEnumerator EnemyStun()
    {
        movementInput = Vector2.zero;
        OnMovementInput?.Invoke(movementInput);
        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;

        if (aiData.currentTarget != null)
        {
            currentCoroutine = StartCoroutine(ChaseAndAttack());
        }
    }
}
