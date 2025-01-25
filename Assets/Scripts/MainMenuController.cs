using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Настройки здоровья")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI элементы")]
    public Slider healthSlider;

    void Start()
    {
        // Устанавливаем начальное здоровье
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        // Уменьшаем здоровье
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Проверяем, если здоровье закончилось
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        // Увеличиваем здоровье
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // Обновляем значение слайдера
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Персонаж погиб!");
        // Здесь можно реализовать смерть персонажа
    }
}
