using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Transform target;
    NavMeshAgent enemy;
    public float speed = 2.0f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    public int damage = 1;
    public int maxHealth = 3;
    private int currentHealth;
    private float originalSpeed;
    private bool isDead = false;
    public List<LootItem> lootTable = new List<LootItem>();

    //private Pathfinding pathfinding;
   // private List<Node> path;
    //private int pathIndex = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //enemy = GetComponent<NavMeshAgent>();
        //enemy.updateRotation = false;
        //enemy.updateUpAxis = false;
        originalSpeed = speed;
        currentHealth = maxHealth;
        //pathfinding = FindObjectOfType<Pathfinding>();
    }

    private void Update()
    {
        /*if (!target)
        {
            GetTarget();
        }
        else
        {
            if (path == null || pathIndex >= path.Count)
            {
                path = pathfinding.FindPath(transform.position, target.position);
                pathIndex = 0;
            }
            if (path != null && pathIndex < path.Count)
            {
               MoveAlongPath();
            }
        }*/
    }

    private void FixedUpdate()
    {
       // rb.velocity = transform.up * speed;
    }

    /*private void MoveAlongPath()
    {
        Node targetNode = path[pathIndex];
        Vector2 targetDirection = ((Vector2)targetNode.worldPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);

        if (Vector2.Distance(transform.position, targetNode.worldPosition) < 0.1f)
        {
            pathIndex++;
        }
    }*/

    /*private void GetTarget()
    {
        enemy.SetDestination(target.position);
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
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

    public void ModifySpeed(float speedFactor)
    {
        speed = originalSpeed * speedFactor;
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
    }

    public bool IsDead()
    {
        return isDead;
    }
}






