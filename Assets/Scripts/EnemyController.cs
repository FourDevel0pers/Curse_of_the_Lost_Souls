using System.Collections;
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
    private bool isAttacking = false;
    private bool isReloading = false;
    private int curWaypointIndex = 0;

    private void Start()
    {
        curWaypointIndex = Random.Range(0, waypoints.childCount);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
        agent.speed = enemyData.walkingSpeed;

        // Збільшуємо поле зору
        enemyData.fieldOfView *= 1.5f;

        target = waypoints.GetChild(curWaypointIndex).position;
        agent.SetDestination(target);
        health = enemyData.health;
    }

    private void FixedUpdate()
    {
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
        }
    }

    private void Patrol()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyData.fieldOfView);
        foreach (Collider col in colliders)
        {
            if (Physics.Raycast(attackPoint.position, col.transform.position - attackPoint.position, out RaycastHit hit, enemyData.fieldOfView))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    StartChasing();
                    break;
                }
            }
        }

        if (Vector3.Distance(transform.position, target) < 1f)
        {
            curWaypointIndex++;
            if (curWaypointIndex >= waypoints.childCount) curWaypointIndex = 0;
            target = waypoints.GetChild(curWaypointIndex).position;
            agent.SetDestination(target);
        }
    }

    private void ChasePlayer()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(player.transform.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(player.transform.position);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < enemyData.attackRange)
        {
            enemyState = EnemyState.Attacking;
        }
    }

    private void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > enemyData.attackRange)
        {
            enemyState = EnemyState.Chasing;
            return;
        }

        if (!isAttacking && !isReloading)
        {
            isAttacking = true;
            agent.isStopped = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, enemyData.attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player.TakeDamage(20); // Наносимо шкоду гравцю
            }
        }
        yield return new WaitForSeconds(enemyData.attackDelay); // Затримка між ударами
        isReloading = false;
        isAttacking = false;
        agent.isStopped = false;
    }

    private void StartChasing()
    {
        agent.speed = enemyData.runningSpeed;
        enemyState = EnemyState.Chasing;
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
