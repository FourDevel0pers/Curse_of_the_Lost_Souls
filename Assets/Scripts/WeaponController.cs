<<<<<<< Updated upstream
﻿using System.Collections;
=======
>>>>>>> Stashed changes
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform shootPoint;
    public Animator animator;
    public Rigidbody weaponRigidbody;
    public Collider weaponCollider;
    public Outline weaponOutline;
    public FirstPersonController player;

    public float shootingSpread;
    public int ammoInMag;
    public int ammo;
    public bool isShooting;
    public bool isReloading;

    // 🔊 Добавленные переменные для звука
    public AudioSource weaponAudioSource;
    public AudioClip shotSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        weaponRigidbody = GetComponent<Rigidbody>();
        weaponCollider = GetComponent<Collider>();
        weaponOutline = GetComponent<Outline>();
        ammoInMag = weaponData.ammoInMag;
        ammo = weaponData.ammo;
        shootingSpread = weaponData.shootingSpread;
    }

    public void Shoot()
    {
<<<<<<< Updated upstream
        if (!isShooting || ammoInMag <= 0)
        {
            isShooting = false;
            return;
        }

        for (int i = 0; i < weaponData.bulletsPerShot; i++)
        {
            GameObject curBullet = Instantiate(weaponData.bulletPrefab, firePoint.position, firePoint.rotation);
            curBullet.transform.Rotate(Random.Range(-shootingSpread, shootingSpread), Random.Range(-shootingSpread, shootingSpread), 0);
            curBullet.GetComponent<Rigidbody>().velocity = curBullet.transform.forward * weaponData.bulletSpeed;
            curBullet.TryGetComponent(out BulletController bullet);
            bullet.damage = weaponData.damage;
            bullet.ownerTag = player.tag;
            Destroy(curBullet, 3);
        }

        ammoInMag--;
        player.playerUI.ammoText.text = $"{ammoInMag} / {ammo}";

        if (!animator.GetBool("Aim"))
        {
            shootingSpread = Mathf.Clamp(shootingSpread + weaponData.shootingSpreadIncreaseValue, weaponData.shootingSpread, weaponData.maxShootingSpread);
            float crossHairSize = ((PlayerUI.resolution.y / 100.0f) * shootingSpread) * 2;
            player.playerUI.crossHair.sizeDelta = new Vector2(crossHairSize, crossHairSize);
=======
        if (ammoInMag <= 0) return;

        ammoInMag--;
        player.UpdateUI();

        if (weaponData.shootSound)
        {
            AudioSource.PlayClipAtPoint(weaponData.shootSound, shootPoint.position);
>>>>>>> Stashed changes
        }

        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
<<<<<<< Updated upstream
            mainCamera.Rotate(-weaponData.verticalSpray, 0, 0);
            mainCamera.parent.Rotate(0, (Random.Range(0, 2) == 0 ? -weaponData.horizontalSpray.leftDirection : weaponData.horizontalSpray.rightDirection), 0);
        }

        animator.SetTrigger("Shoot");


            weaponAudioSource.clip = shotSound;
        weaponAudioSource.Play();

        if (weaponData.fireRate > 0)
        {
            Invoke(nameof(Shoot), 60.0f / weaponData.fireRate);
        }
=======
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>()?.TakeDamage(weaponData.damage);
            }
        }

        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(weaponData.fireRate);
        if (isShooting) Shoot();
>>>>>>> Stashed changes
    }

    public IEnumerator Reload()
    {
<<<<<<< Updated upstream
        if (ammoInMag >= weaponData.ammoInMag || ammo <= 0) yield break;

        animator.SetTrigger("Reload");
        animator.SetBool("Aim", false);
        isShooting = false;
        isReloading = true;

        yield return new WaitForSeconds(weaponData.reloadingDuration);

        int requiredAmmo = Mathf.Min(weaponData.ammoInMag - ammoInMag, ammo);
        ammoInMag += requiredAmmo;
        ammo -= requiredAmmo;

        player.playerUI.ammoText.text = $"{ammoInMag} / {ammo}";
=======
        if (isReloading || ammo <= 0 || ammoInMag == weaponData.ammoInMag) yield break;

        isReloading = true;
        animator.SetTrigger("Reload");

        yield return new WaitForSeconds(weaponData.reloadingDuration);

        int ammoToReload = Mathf.Min(weaponData.ammoInMag - ammoInMag, ammo);
        ammoInMag += ammoToReload;
        ammo -= ammoToReload;
>>>>>>> Stashed changes
        isReloading = false;

        player.UpdateUI();
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
