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

    [SerializeField] private List<GameObject> dropPrefabs;

    [HideInInspector] public FirstPersonController player;
    private NavMeshAgent agent;
    public Animator animator;
    private Vector3 target;
    private bool isAttacking = false;
    private bool isReloading = false;
    private int curWaypointIndex = 0;
    [SerializeField] private AudioSource audio;

    private bool isDead = false;

    private void Start()
    {
        curWaypointIndex = Random.Range(0, waypoints.childCount);
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<FirstPersonController>();
        agent.speed = enemyData.walkingSpeed;

        target = waypoints.GetChild(curWaypointIndex).position;
        agent.SetDestination(target);
        animator.SetBool("IsWalking", true); // �������� �������� ������
        health = enemyData.health;

        enemyState = EnemyState.Patroling; // ������������� ��������� ��������� ��� ��������������
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
        // ���������, ������ �� ���� ������� ����
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            curWaypointIndex++;
            if (curWaypointIndex >= waypoints.childCount) curWaypointIndex = 0;
            target = waypoints.GetChild(curWaypointIndex).position;
            agent.SetDestination(target);
        }

        // ��������� ������� ������ � ���� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyData.fieldOfView);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                if (Physics.Raycast(attackPoint.position, col.transform.position - attackPoint.position, out RaycastHit hit, enemyData.fieldOfView))
                {
                    StartChasing();
                    break;
                }
            }
        }
    }

    private void ChasePlayer()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(player.transform.position, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(player.transform.position);
            animator.SetBool("IsWalking", true);
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
            animator.SetBool("IsWalking", false);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("Attack");
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, enemyData.attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player.TakeDamage(enemyData.damage); // ������� ���� ������
            }
        }
        yield return new WaitForSeconds(enemyData.attackDelay); // �������� ����� �������
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
        if (isDead) return;
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        foreach (GameObject drop in dropPrefabs)
        {
            Instantiate(drop, transform.position + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f)), Random.rotation);
        }
        agent.isStopped = true;
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Die");
        Destroy(GetComponent<Collider>());
        audio.Stop();
        Destroy(gameObject, 30f); // ������� ����� ����� 30 ������ (��� ��������� ��������)
        Destroy(this);
    }

    // ��������� ��������� ���� (��� ������������� Collider)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // ���������, ���� ��� ����
        {
            TakeDamage(20); // ������� ����
            Destroy(other.gameObject); // ���������� ����
        }
    }
}

