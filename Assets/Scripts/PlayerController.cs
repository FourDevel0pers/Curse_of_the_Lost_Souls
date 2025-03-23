using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки игрока")]
    public float health = 100f;
    public float moveSpeed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;

    [Header("UI элементы")]
    public Slider healthSlider;

    [Header("Настройки оружия")]
    public Transform weaponHolder;
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public float dropForce = 5f; // Увеличил для более заметного выброса

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private GameObject currentWeapon;

    private void Start()
    {
        // Инициализация CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController не найден на объекте игрока!");
            return;
        }

        // Настройка курсора
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Инициализация UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
        else
        {
            Debug.LogWarning("HealthSlider не назначен в инспекторе!");
        }

        // Проверка WeaponHolder
        if (weaponHolder == null)
        {
            Debug.LogWarning("WeaponHolder не назначен в инспекторе!");
        }
    }

    private void Update()
    {
        if (controller == null) return;

        MovePlayer();

        // Поднятие и выброс оружия
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickupWeapon();
        }
        if (Input.GetKeyDown(dropKey))
        {
            DropWeapon();
        }

        // Тестовый урон
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    private void MovePlayer()
    {
        // Проверка нахождения на земле
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Ввод для движения
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
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

    private void TryPickupWeapon()
    {
        if (currentWeapon != null || weaponHolder == null)
        {
            Debug.Log("Уже держите оружие или WeaponHolder не назначен!");
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Главная камера не найдена!");
            return;
        }

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                GameObject weapon = hit.collider.gameObject;
                PickupWeapon(weapon);
                Debug.Log("Оружие поднято: " + weapon.name);
            }
            else
            {
                Debug.Log("Объект не является оружием: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("Ничего не найдено в радиусе " + pickupRange);
        }
    }

    private void PickupWeapon(GameObject weapon)
    {
        currentWeapon = weapon;
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else
        {
            Debug.LogWarning("У оружия " + weapon.name + " отсутствует Rigidbody!");
        }

        Collider col = weapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        else
        {
            Debug.LogWarning("У оружия " + weapon.name + " отсутствует Collider!");
        }
    }

    private void DropWeapon()
    {
        if (currentWeapon == null || weaponHolder == null)
        {
            Debug.Log("Нет оружия для выброса!");
            return;
        }

        GameObject weaponToDrop = currentWeapon;
        weaponToDrop.transform.SetParent(null);
        currentWeapon = null;

        Rigidbody rb = weaponToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * dropForce + Vector3.up * 0.5f, ForceMode.Impulse);
            Debug.Log("Оружие выброшено: " + weaponToDrop.name + " с силой " + dropForce);
        }
        else
        {
            Debug.LogWarning("У выброшенного оружия отсутствует Rigidbody!");
        }

        Collider col = weaponToDrop.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(0, health - damage);
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        if (currentWeapon != null)
        {
            DropWeapon();
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * pickupRange);
    }
}