using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WeaponController curWeapon;
    public float health = 100f;  // ������������ ��������� �������� HP �� 100
    public float moveSpeed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 1.5f;

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
                //Weapon
        if (!curWeapon) return;
        if(Input.GetMouseButtonDown(1))
        {
            curWeapon.animator.SetBool("Aim", true);
            // playerUI.crossHair.gameObject.SetActive(false);
            // playerUI.alternateCrossHair.gameObject.SetActive(false);
            curWeapon.shootingSpread = curWeapon.weaponData.aimShootingSpread;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            curWeapon.animator.SetBool("Aim", false);
            // playerUI.crossHair.gameObject.SetActive(true);
            // playerUI.alternateCrossHair.gameObject.SetActive(true);
            curWeapon.shootingSpread = curWeapon.weaponData.shootingSpread;
        }

        if(Input.GetMouseButtonDown(0) && !curWeapon.isShooting && !curWeapon.isReloading)
        {
            curWeapon.isShooting = true;
            curWeapon.Shoot();
        }
        else if(Input.GetMouseButtonUp(0) && curWeapon.isShooting)
        {
            curWeapon.isShooting = false;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            curWeapon.StartCoroutine(curWeapon.Reload());
        }
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
