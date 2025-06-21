

using UnityEngine;
using System.Collections;

public class MeleeWeaponController : MonoBehaviour
{
    public MeleeWeaponData weaponData;
    public Transform attackPoint;

    [HideInInspector] public FirstPersonController player;
    public Animator animator { get; private set; }
    public Rigidbody weaponRigidbody { get; private set; }
    public Collider weaponCollider { get; private set; }
    public Outline weaponOutline { get; private set; }
    [HideInInspector] public bool isAttacking = false;

    private bool canAttack = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        weaponRigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<Collider>();
        weaponOutline = GetComponent<Outline>();
    }

    public void Attack()
    {
        if (!canAttack || isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("MeleeAttack");

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, weaponData.attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                if (hit.TryGetComponent(out EnemyController enemy))
                {
                    enemy.TakeDamage(weaponData.damage);
                }
            }
            else if (hit.CompareTag("Boss"))
            {
                if (hit.TryGetComponent(out BossController boss))
                {
                    boss.TakeDamage(weaponData.damage);
                }
            }
        }

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(weaponData.attackCooldown);
        canAttack = true;
        isAttacking = false;
    }

    public void EnablePhysics()
    {
        if (weaponRigidbody) weaponRigidbody.isKinematic = false;
        if (weaponCollider) weaponCollider.enabled = true;
    }

    public void DisablePhysics()
    {
        if (weaponRigidbody) weaponRigidbody.isKinematic = true;
        if (weaponCollider) weaponCollider.enabled = false;
    }

    public void EnableOutline()
    {
        if (weaponOutline) weaponOutline.enabled = true;
    }

    public void DisableOutline()
    {
        if (weaponOutline) weaponOutline.enabled = false;
    }
}
