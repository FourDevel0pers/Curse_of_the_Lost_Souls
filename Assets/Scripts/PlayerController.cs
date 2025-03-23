using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("��������� ������")]
    public float health = 100f;
    public float moveSpeed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;

    [Header("UI ��������")]
    public Slider healthSlider;

    [Header("��������� ������")]
    public Transform weaponHolder;
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public float dropForce = 5f; // �������� ��� ����� ��������� �������

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private GameObject currentWeapon;

    private void Start()
    {
        // ������������� CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController �� ������ �� ������� ������!");
            return;
        }

        // ��������� �������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ������������� UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
        else
        {
            Debug.LogWarning("HealthSlider �� �������� � ����������!");
        }

        // �������� WeaponHolder
        if (weaponHolder == null)
        {
            Debug.LogWarning("WeaponHolder �� �������� � ����������!");
        }
    }

    private void Update()
    {
        if (controller == null) return;

        MovePlayer();

        // �������� � ������ ������
        if (Input.GetKeyDown(pickupKey))
        {
            TryPickupWeapon();
        }
        if (Input.GetKeyDown(dropKey))
        {
            DropWeapon();
        }

        // �������� ����
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    private void MovePlayer()
    {
        // �������� ���������� �� �����
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ���� ��� ��������
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // ������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        // ����������
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void TryPickupWeapon()
    {
        if (currentWeapon != null || weaponHolder == null)
        {
            Debug.Log("��� ������� ������ ��� WeaponHolder �� ��������!");
            return;
        }

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("������� ������ �� �������!");
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
                Debug.Log("������ �������: " + weapon.name);
            }
            else
            {
                Debug.Log("������ �� �������� �������: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("������ �� ������� � ������� " + pickupRange);
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
            Debug.LogWarning("� ������ " + weapon.name + " ����������� Rigidbody!");
        }

        Collider col = weapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        else
        {
            Debug.LogWarning("� ������ " + weapon.name + " ����������� Collider!");
        }
    }

    private void DropWeapon()
    {
        if (currentWeapon == null || weaponHolder == null)
        {
            Debug.Log("��� ������ ��� �������!");
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
            Debug.Log("������ ���������: " + weaponToDrop.name + " � ����� " + dropForce);
        }
        else
        {
            Debug.LogWarning("� ������������ ������ ����������� Rigidbody!");
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
        Debug.Log("����� �����!");
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