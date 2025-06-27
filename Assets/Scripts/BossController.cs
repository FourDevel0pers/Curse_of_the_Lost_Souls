using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{
    Idle,
    Chasing,
    Attacking,
    SpecialAttack
}

public class BossController : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 500;
    public float currentHealth;
    public float phase2Threshold = 250;
    public float attackRange = 3f;
    public float specialAttackCooldown = 10f;

    public float damage = 30f;
    public float specialAttackDamage = 50f;
    public float attackDelay = 2f;
    public float movementSpeed = 4f;
    public float chaseRange = 15f;

    public Transform attackPoint;
    public GameObject specialAttackEffect;

    private BossState bossState;
    private NavMeshAgent agent;
    private FirstPersonController player;
    private Animator animator;

    private bool isAttacking = false;
    private bool isSpecialReady = true;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        bossState = BossState.Idle;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<FirstPersonController>();

        agent.speed = movementSpeed;
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (bossState)
        {
            case BossState.Idle:
                if (distanceToPlayer <= chaseRange)
                    bossState = BossState.Chasing;
                break;

            case BossState.Chasing:
                agent.SetDestination(player.transform.position);
                animator.SetBool("IsWalking", true);

                if (distanceToPlayer <= attackRange)
                {
                    bossState = BossState.Attacking;
                }
                break;

            case BossState.Attacking:
                if (!isAttacking)
                    StartCoroutine(Attack());

                if (distanceToPlayer > attackRange)
                {
                    bossState = BossState.Chasing;
                }
                break;

            case BossState.SpecialAttack:
                break;
        }

        if (currentHealth <= phase2Threshold && isSpecialReady)
        {
            StartCoroutine(PerformSpecialAttack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); 

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        agent.isStopped = false;
    }

    private IEnumerator PerformSpecialAttack()
    {
        isSpecialReady = false;
        bossState = BossState.SpecialAttack;
        agent.isStopped = true;

        animator.SetTrigger("SpecialAttack");
        yield return new WaitForSeconds(1.2f);

        Instantiate(specialAttackEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player.TakeDamage(specialAttackDamage);
            }
        }

        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        bossState = BossState.Chasing;

        yield return new WaitForSeconds(specialAttackCooldown);
        isSpecialReady = true;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(GetComponent<Collider>());
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(20);
            Destroy(other.gameObject);
        }
    }
}