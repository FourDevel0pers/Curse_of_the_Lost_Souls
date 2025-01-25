using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI

public class PlayerController : MonoBehaviour
{
    [Header("��������� ������")]
    public float health = 100f; // ��������� �������� ��������
    public float moveSpeed = 5.0f; // �������� ��������
    public float sprintMultiplier = 1.5f; // ��������� �������� ��� ����
    public float jumpHeight = 1.5f; // ������ ������

    [Header("UI ��������")]
    public Slider healthSlider; // ������� ��� ����������� ��������

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        // �������� ��������� CharacterController
        controller = GetComponent<CharacterController>();

        // �������� ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �������������� ������� ��������
        if (healthSlider != null)
        {
            healthSlider.maxValue = health; // ������������� ������������ ��������
            healthSlider.value = health;   // ������������� ������� ��������
        }
    }

    private void Update()
    {
        MovePlayer();

        // ��� �����: ��������� �������� �� 10 ��� ������� ������� H
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    private void MovePlayer()
    {
        // ���������, �� ����� �� �����
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // �������� ���� ������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ������������ ���
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // ��������
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
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

    public void TakeDamage(float damage)
    {
        health -= damage;

        // ��������� ������� ��������
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }

        // ���������, ���� �������� ���������� �� 0
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("����� �����!");
        Destroy(gameObject); // ������� ������ ������
    }
}
