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
        agent.speed = enemyData.walkingSpeed;
        target = waypoints.GetChild(curWaypointIndex).position;
        agent.SetDestination(target);
        health = enemyData.health;
    }

    private void FixedUpdate()
    {
        if (isStunned) return;

        switch (enemyState)
        {
            case EnemyState.Patroling:
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
                break;

            case EnemyState.Chasing:
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    if (Vector3.Distance(attackPoint.position, playerObject.transform.position) < enemyData.attackRange)
                        enemyState = EnemyState.Attacking;
                    agent.SetDestination(playerObject.transform.position);
                }
                break;

            case EnemyState.Attacking:
                GameObject attackingPlayer = GameObject.FindGameObjectWithTag("Player");
                if (attackingPlayer != null)
                {
                    Rotate(attackingPlayer.transform.position - transform.position, transform);
                    if (Vector3.Distance(attackPoint.position, attackingPlayer.transform.position) < enemyData.attackRange)
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
                return;

            default:
                return;
        }
    }

    private IEnumerator Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, enemyData.attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerController player = collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(enemyData.damage);
                }
            }
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

    private void StartChasing()
    {
        if (enemyState != EnemyState.Patroling) return;
        agent.speed = enemyData.runningSpeed;
        enemyState = EnemyState.Chasing;
    }

    private void CallEnemies()
    {
        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
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
}
