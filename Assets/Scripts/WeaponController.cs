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
    public int maxAmmoInMag = 30;
    public int maxAmmo = 300;
    public int currentAmmo;                
    public float reloadTime = 2f;           
    private bool isReloading = false;       
    private float currentSpread = 0.0f;     
    private float nextFireTime = 0.0f;       
    public float fireRate = 0.1f;           

    void Start()
    {
        currentAmmo = maxAmmoInMag;
    }

    void Update()
    {
        if (isReloading) return; 

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                nextFireTime = Time.time + fireRate;  
                ShootProjectile();                    
            }
        }

        if (!Input.GetButton("Fire1") && currentSpread > 0)
        {
            currentSpread = Mathf.Max(currentSpread - recoverySpeed * Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading) StartCoroutine(Reload());
    }

    private void ShootProjectile()
    {
        currentAmmo--; 
        currentSpread = Mathf.Min(currentSpread + spreadIncrease, maxSpread);
        Vector3 shootDirection = Camera.main.transform.forward;
        shootDirection.x += Random.Range(-currentSpread, currentSpread) * Mathf.Deg2Rad;
        shootDirection.y += Random.Range(-currentSpread, currentSpread) * Mathf.Deg2Rad;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.transform.forward = shootDirection.normalized;
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
    }

    private IEnumerator Reload()
    {
        if (maxAmmo == 0 || currentAmmo >= maxAmmoInMag) yield break;
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        int requiredAmmo = Mathf.Min(maxAmmo, maxAmmoInMag - currentAmmo);
        if (maxAmmo < 0) requiredAmmo = currentAmmo - maxAmmoInMag;
        else maxAmmo -= requiredAmmo;
        currentAmmo += requiredAmmo;
        isReloading = false;
    }
}