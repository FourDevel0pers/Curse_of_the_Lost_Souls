using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float damage = 20f;  // Урон, который наносится врагу
    public float range = 100f;   // Дальность выстрела
    public Camera playerCamera;  // Камера игрока
    public ParticleSystem gunfireEffect;  // Эффект выстрела

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Стрельба при нажатии кнопки (по умолчанию это левая кнопка мыши)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        gunfireEffect.Play();  // Запуск эффекта выстрела

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            //EnemyController enemy = hit.collider.GetComponent<EnemyController>();  // Пытаемся найти врага по его компоненту
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);  // Если это враг, наносим урон
            //}
        }
    }
}
