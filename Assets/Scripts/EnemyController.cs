using DG.Tweening;
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
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyState enemyState;
    public Transform attackPoint;
    public Transform waypoints;

    [HideInInspector] public PlayerController player;
    private NavMeshAgent agent;
    private Animator animator;
    private LevelController level;
    private Vector3 target;
    private Transform distraction;
    private bool isAttacking = false;
    private bool isReloading = false;
    private bool isStunned = false;
    private int curWaypointIndex = 0;

    private void Start()
    {
        curWaypointIndex = Random.Range(0, waypoints.childCount);
        level = FindFirstObjectByType<LevelController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
        agent.speed = enemyData.walkingSpeed;
        target = waypoints.GetChild(curWaypointIndex).position;
        agent.SetDestination(target);
    }

    private void FixedUpdate()
    {
        if (isStunned) return;
        if (distraction)
        {
            if (Vector3.Distance(attackPoint.position, distraction.position) < enemyData.attackRange) StartCoroutine(Stun(3, "Inspect"));
            return;
        }
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
                if (Vector3.Distance(attackPoint.position, player.transform.position) < enemyData.attackRange)
                    enemyState = EnemyState.Attacking;
                agent.SetDestination(player.transform.position);
                break;
            case EnemyState.Attacking:
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
                return;
            default:
                return;
        }

        //if (!animator.GetBool("Move") && !isAttacking) animator.SetBool("Move", true);
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
        foreach (Transform enemy in level.enemies)
        {
            enemy.GetComponent<EnemyController>().StartChasing();
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
            if (collider.CompareTag("Player")) player.Die();
        }
        yield return new WaitForSeconds(enemyData.attackDelay);
        if (isAttacking) StartCoroutine(Attack());
    }
}