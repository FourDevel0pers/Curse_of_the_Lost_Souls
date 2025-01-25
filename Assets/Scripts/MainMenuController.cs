using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("��������� ��������")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI ��������")]
    public Slider healthSlider;

    void Start()
    {
        // ������������� ��������� ��������
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        // ��������� ��������
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // ���������, ���� �������� �����������
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        // ����������� ��������
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // ��������� �������� ��������
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("�������� �����!");
        // ����� ����� ����������� ������ ���������
    }
}
