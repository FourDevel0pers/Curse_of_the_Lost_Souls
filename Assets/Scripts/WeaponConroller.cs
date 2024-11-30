using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float pushingForce;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider weaponCollider;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<Collider>();
    }

    public void SwingLeft()
    {
        animator.SetTrigger("SwingLeft");
    }

    public void SwingDown()
    {
        animator.SetTrigger("SwingDown");
    }

    public void Stab()
    {
        animator.SetTrigger("Stab");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out EnemyController enemy))
        {
            enemy.TakeDamage(damage);
        }
        if (collider.TryGetComponent(out Rigidbody rb))
        {
            rb.AddExplosionForce(pushingForce, transform.position, 2);
        }
    }
}