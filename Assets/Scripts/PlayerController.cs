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

    [Header("Настройки оружия и предметов")]
    public Transform weaponHolder;
    public Transform handHolder;  // Новое — держатель предметов
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public float dropForce = 5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private GameObject currentItem;  // Новое — текущий предмет в руках
    private Transform mainCamera;


    private void Start()
    {
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController не найден на объекте игрока!");
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }

        if (weaponHolder == null)
        {
            Debug.LogWarning("WeaponHolder не назначен в инспекторе!");
        }
        if (handHolder == null)
        {
            Debug.LogWarning("HandHolder не назначен в инспекторе!");
        }
    }

    private void Update()
    {
        if (controller == null) return;

        MovePlayer();

        if (Input.GetKeyDown(pickupKey))
        {
            TryPickupWeapon();
            TryPickupItem(); // Теперь игрок может поднимать предметы в руки
        }

        if (Input.GetKeyDown(dropKey))
        {
            DropWeapon();
            DropItem(); // Теперь можно выбрасывать предметы
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    private void MovePlayer()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void TryPickupWeapon()
    {
        if (currentWeapon != null || weaponHolder == null) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                PickupWeapon(hit.collider.gameObject);
            }
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

        Collider col = weapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void DropWeapon()
    {
        if (currentWeapon == null || weaponHolder == null) return;

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
        }

        Collider col = weaponToDrop.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    private void TryPickupItem()
    {
        if (currentItem != null || handHolder == null) return;

        if (Physics.Raycast(mainCamera.position,mainCamera.forward, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("Item"))
            {
                PickupItem(hit.collider.gameObject);
            }
        }
    }

    private void PickupItem(GameObject item)
    {
        if (currentItem != null)
        {
            DropItem();
        }

        currentItem = item;
        item.transform.SetParent(handHolder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = item.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log("Предмет в руках: " + item.name);
    }

    private void DropItem()
    {
        if (currentItem == null || handHolder == null) return;

        GameObject itemToDrop = currentItem;
        itemToDrop.transform.SetParent(null);
        currentItem = null;

        Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.forward * dropForce + Vector3.up * 0.5f, ForceMode.Impulse);
        }

        Collider col = itemToDrop.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        Debug.Log("Предмет выброшен: " + itemToDrop.name);
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
        if (currentItem != null)
        {
            DropItem();
        }
        Destroy(gameObject);
    }
}
