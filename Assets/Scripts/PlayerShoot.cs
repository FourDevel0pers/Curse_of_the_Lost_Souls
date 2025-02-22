using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float damage = 20f;  // ����, ������� ��������� �����
    public float range = 100f;   // ��������� ��������
    public Camera playerCamera;  // ������ ������
    public ParticleSystem gunfireEffect;  // ������ ��������

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // �������� ��� ������� ������ (�� ��������� ��� ����� ������ ����)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        gunfireEffect.Play();  // ������ ������� ��������

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            //EnemyController enemy = hit.collider.GetComponent<EnemyController>();  // �������� ����� ����� �� ��� ����������
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);  // ���� ��� ����, ������� ����
            //}
        }
    }
}
