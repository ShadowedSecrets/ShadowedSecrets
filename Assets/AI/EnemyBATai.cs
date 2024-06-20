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

    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    bool following = false;

    private Rigidbody2D rb;
    public int damage = 1;
    public int maxHealth = 3;
    private int currentHealth;

    private bool isDead = false;
    public List<LootItem> lootTable = new List<LootItem>();

    // Freeze variables
    private bool isFrozen = false;
    private float freezeDuration;
    private Coroutine currentCoroutine;

    private void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);

        // Ensure Rigidbody2D is assigned
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }
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

        //Enemy AI movement based on Target availability
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
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
            }
        }
        following = false;
    }

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
            TakeDamage(1);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayEnemyHitSound();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
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
}
