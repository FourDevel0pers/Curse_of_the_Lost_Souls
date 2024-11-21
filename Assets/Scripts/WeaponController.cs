using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float projectileSpeed = 20f;     
    public GameObject projectilePrefab;  
    public Transform firePoint;              
    public float spreadIncrease = 0.5f;      
    public float maxSpread = 5f;            
    public float recoverySpeed = 2f;        
    private float currentSpread = 0.0f;     
    private float nextFireTime = 0.0f;       
    public float fireRate = 0.1f;           

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;  
            ShootProjectile();                    
        }

        if (!Input.GetButton("Fire1") && currentSpread > 0)
        {
            currentSpread = Mathf.Max(currentSpread - recoverySpeed * Time.deltaTime, 0);
        }
    }

    private void ShootProjectile()
    {
        currentSpread = Mathf.Min(currentSpread + spreadIncrease, maxSpread);
        Vector3 shootDirection = Camera.main.transform.forward;
        shootDirection.x += Random.Range(-currentSpread, currentSpread) * Mathf.Deg2Rad;
        shootDirection.y += Random.Range(-currentSpread, currentSpread) * Mathf.Deg2Rad;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.transform.forward = shootDirection.normalized;
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
    }
}