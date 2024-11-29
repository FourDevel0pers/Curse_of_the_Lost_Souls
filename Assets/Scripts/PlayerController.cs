using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health;
    public float moveSpeed = 5.0f;
    public float sprintMultiplier = 1.5f; 
    public float jumpHeight = 1.5f;        
    public GameObject projectilePrefab;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Параметри для підйому предметів
    public Transform holdPoint; // Точка, де тримається предмет
    public float pickUpRange = 3f; // Дальність захоплення
    public float throwForce = 10f; // Сила кидка
    private GameObject heldObject;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovePlayer();
        ShootProjectile();
        HandlePickUpAndThrow();
    }

    private void MovePlayer()
    {
        isGrounded = controller.isGrounded;  

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ShootProjectile()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + Camera.main.transform.forward, Quaternion.identity);
            projectile.transform.forward = Camera.main.transform.forward;
        }
    }

    private void HandlePickUpAndThrow()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Підняти або кинути
        {
            if (heldObject == null)
            {
                // Спроба підняти об'єкт
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpRange))
                {
                    if (hit.collider.CompareTag("PickUp")) // Перевіряємо тег
                    {
                        PickUpObject(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                // Викинути об'єкт
                ThrowObject();
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        obj.GetComponent<Rigidbody>().isKinematic = true; // Вимикаємо фізику
        obj.transform.SetParent(holdPoint); // Прив'язуємо до гравця
        obj.transform.localPosition = Vector3.zero; // Центруємо в точці
    }

    private void ThrowObject()
    {
        Rigidbody objRigidbody = heldObject.GetComponent<Rigidbody>();
        heldObject.transform.SetParent(null); // Від'єднуємо від гравця
        objRigidbody.isKinematic = false; // Вмикаємо фізику
        objRigidbody.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse); // Додаємо імпульс
        heldObject = null; // Очищаємо посилання
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die(); 
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
