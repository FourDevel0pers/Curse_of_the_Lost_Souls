using UnityEngine;

public class Projectile : MonoBehaviour
{  
    public float lifetime = 2f;    
    public float damage = 10f;     

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
            }
        }

        Destroy(gameObject);
    }
}