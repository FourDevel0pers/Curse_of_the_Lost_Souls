using UnityEngine;
using UnityEngine.UI; // Для работы с UI

public class PlayerController : MonoBehaviour
{
    [Header("Настройки игрока")]
    public float health = 100f; // Начальное значение здоровья
    public float moveSpeed = 5.0f; // Скорость движения
    public float sprintMultiplier = 1.5f; // Множитель скорости при беге
    public float jumpHeight = 1.5f; // Высота прыжка

    [Header("UI элементы")]
    public Slider healthSlider; // Слайдер для отображения здоровья

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        // Получаем компонент CharacterController
        controller = GetComponent<CharacterController>();

        // Скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Инициализируем слайдер здоровья
        if (healthSlider != null)
        {
            healthSlider.maxValue = health; // Устанавливаем максимальное значение
            healthSlider.value = health;   // Устанавливаем текущее значение
        }
    }

    private void Update()
    {
        MovePlayer();

        // Для теста: уменьшаем здоровье на 10 при нажатии клавиши H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    private void MovePlayer()
    {
        // Проверяем, на земле ли игрок
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Получаем ввод игрока
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Обрабатываем бег
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // Движение
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        // Гравитация
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        // Обновляем слайдер здоровья
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        // Проверяем, если здоровье опустилось до 0
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        Destroy(gameObject); // Удаляем объект игрока
    }
}
