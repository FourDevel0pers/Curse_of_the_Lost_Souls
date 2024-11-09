using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; 
    public float sprintMultiplier = 1.5f;  // Прискорення
    public float jumpHeight = 1.5f;        // Висота стрибка
    public GameObject projectilePrefab;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

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

        // Додаємо гравітацію
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
}