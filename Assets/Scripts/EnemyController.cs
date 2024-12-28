using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patroling,
    Chasing,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyState enemyState;
    public Transform attackPoint;
    public Transform waypoints;

    [HideInInspector] public PlayerController player;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 target;
    private Transform distraction;
    private bool isAttacking = false;
    private bool isReloading = false;
    private bool isStunned = false;
    private int curWaypointIndex = 0;

    private void Start()
    {
        curWaypointIndex = Random.Range(0, waypoints.childCount);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
        agent.speed = enemyData.walkingSpeed;
        target = waypoints.GetChild(curWaypointIndex).position;
        agent.SetDestination(target);
        health = enemyData.health;
    }

    private void FixedUpdate()
    {
        if (isStunned) return;

        if (distraction)
        {
            if (Vector3.Distance(attackPoint.position, distraction.position) < enemyData.attackRange)
                StartCoroutine(Stun(3, "Inspect"));
            return;
        }

        switch (enemyState)
        {
            case EnemyState.Patroling:
                Patrol();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
            default:
                break;
        }
    }

    private void Patrol()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyData.fieldOfView);
        foreach (Collider col in colliders)
        {
            if (Physics.Raycast(attackPoint.position, col.transform.position - attackPoint.position, out RaycastHit hit, enemyData.fieldOfView))
            {
                Debug.DrawLine(attackPoint.position, hit.point, Color.green, 1f);
                if (hit.collider.CompareTag("Player"))
                {
                    StartChasing();
                    CallEnemies();
                    break;
                }
            }
        }

        if (Vector3.Distance(attackPoint.position, target) < enemyData.attackRange)
        {
            curWaypointIndex++;
            if (curWaypointIndex > waypoints.childCount - 1) curWaypointIndex = 0;
            target = waypoints.GetChild(curWaypointIndex).position;
            agent.SetDestination(target);
        }
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - attackPoint.position;

        if (!Physics.Raycast(attackPoint.position, directionToPlayer, out RaycastHit hit, enemyData.fieldOfView) || 
            hit.collider.CompareTag("Player"))
        {
            agent.SetDestination(player.transform.position); 
        }
        else
        {
            agent.SetDestination(hit.point + hit.normal * 2.0f); 
        }

        if (Vector3.Distance(attackPoint.position, player.transform.position) < enemyData.attackRange)
        {
            enemyState = EnemyState.Attacking;
        }
    }

    private void AttackPlayer()
    {
        Rotate(player.transform.position - transform.position, transform);

        if (Vector3.Distance(attackPoint.position, player.transform.position) < enemyData.attackRange)
        {
            if (!isAttacking && !isReloading)
            {
                isAttacking = true;
                agent.isStopped = true;
                StartCoroutine(Attack());
            }
        }
        else if (isAttacking)
        {
            isAttacking = false;
            agent.isStopped = false;
            StopCoroutine(Attack());
            enemyState = EnemyState.Chasing;
        }
    }

    public void GoToDistraction(Transform distractionObject)
    {
        if (enemyState != EnemyState.Patroling) return;
        distraction = distractionObject;
        agent.SetDestination(distraction.position);
    }

    public void StartChasing()
    {
        if (enemyState != EnemyState.Patroling) return;
        agent.speed = enemyData.runningSpeed;
        enemyState = EnemyState.Chasing;
    }

    private void CallEnemies()
    {
        foreach (EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None))
        {
            enemy.StartChasing();
        }
    }

    private void Rotate(Vector3 rotate, Transform objectOfRotation)
    {
        rotate.y = 0;
        Quaternion rotation = Quaternion.LookRotation(rotate, Vector3.up);
        objectOfRotation.rotation = Quaternion.Slerp(objectOfRotation.rotation, rotation, Time.deltaTime * agent.angularSpeed);
    }

    public IEnumerator Stun(float duration, string animationTriggerName = "Stun")
    {
        StopCoroutine(Stun(0));
        //animator.SetTrigger(animationTriggerName);
        isStunned = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    private IEnumerator Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, enemyData.attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player")) player.TakeDamage(enemyData.damage);
        }
        yield return new WaitForSeconds(enemyData.attackDelay);
        if (isAttacking) StartCoroutine(Attack());
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
