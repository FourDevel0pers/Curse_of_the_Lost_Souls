using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerStats
{
    public float health;

    public float acceleration;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpForce;
    public float throwingForce;

    public float sensitivity;
    public float minCameraAngle;
    public float maxCameraAngle;

    public float reachingDistance;
    public LayerMask interactionLayer;
}

[System.Serializable]
public class PlayerResources
{
    public int stone;
    public int wood;
}

[System.Serializable]
public class Controls
{
    public KeyCode interact;
    public KeyCode run;
    public KeyCode drop;
}

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerResources resources;
    public Controls controls;

    public WeaponController curWeapon;
    public Transform weaponPoint;

    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public GameObject interactionObject;

    private float health;
    private float speed;
    Camera mainCamera;
    Rigidbody rb;

    private void Start()
    {
        health = playerStats.health;
        speed = playerStats.walkingSpeed;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(controls.run)) speed = playerStats.runningSpeed;
        if (Input.GetKeyUp(controls.interact)) speed = playerStats.walkingSpeed;
        float moveX = Input.GetAxis("Horizontal") * playerStats.acceleration;
        float moveY = Input.GetAxis("Jump") * (isGrounded ? playerStats.jumpForce : 0f);
        float moveZ = Input.GetAxis("Vertical") * playerStats.acceleration;
        float rotationX = Input.GetAxis("Mouse X") * playerStats.sensitivity * Time.timeScale;
        float rotationY = -Input.GetAxis("Mouse Y") * playerStats.sensitivity * Time.timeScale;

        rb.AddForce(transform.forward * moveZ + transform.right * moveX + transform.up * moveY);
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -speed, speed));
        transform.Rotate(0, rotationX, 0);

        float curRotY = mainCamera.transform.localEulerAngles.x + rotationY;
        if (curRotY > 180) curRotY -= 360;
        curRotY = Mathf.Clamp(curRotY, playerStats.minCameraAngle, playerStats.maxCameraAngle);
        if (curRotY < 0) curRotY += 360;
        mainCamera.transform.localEulerAngles = new Vector3(curRotY, mainCamera.transform.localEulerAngles.y, 0);

        if (Input.GetKeyDown(controls.interact)) Interact();
        if (Input.GetKeyDown(controls.drop)) DropWeapon();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hitInfo, playerStats.reachingDistance, playerStats.interactionLayer))
        {
            if (interactionObject && interactionObject.TryGetComponent(out Outline outline)) outline.enabled = false;
            interactionObject = hitInfo.transform.gameObject;
            if (hitInfo.transform.TryGetComponent(out outline))
            {
                outline.enabled = true;
            }
        }
        else
        {
            if (interactionObject && interactionObject.TryGetComponent(out Outline outline)) outline.enabled = false;
            interactionObject = null;
        }
    }

    private void Interact()
    {
        if (!interactionObject) return;
        switch (interactionObject.tag)
        {
            case "Weapon":
                PickUpWeapon();
                break;
            case "Resource":
                if (interactionObject.name.ToLower().Contains(nameof(PlayerResources.stone))) resources.stone++;
                else if (interactionObject.name.ToLower().Contains(nameof(PlayerResources.wood))) resources.wood++;
                Destroy(interactionObject);
                break;
            case "Door":
                break;
        }
    }

    private void PickUpWeapon()
    {
        if (curWeapon) DropWeapon();
        if (interactionObject.TryGetComponent(out Outline outline)) outline.enabled = false;
        curWeapon = interactionObject.GetComponent<WeaponController>();
        curWeapon.rb.isKinematic = true;
        curWeapon.weaponCollider.enabled = false;
        curWeapon.transform.SetParent(weaponPoint);
        curWeapon.transform.position = weaponPoint.position;
        curWeapon.transform.rotation = weaponPoint.rotation;
    }

    private void DropWeapon()
    {
        if (!curWeapon) return;
        curWeapon.rb.isKinematic = false;
        curWeapon.weaponCollider.enabled = true;
        curWeapon.transform.SetParent(null);
        curWeapon.rb.AddForce(weaponPoint.forward * playerStats.throwingForce, ForceMode.VelocityChange);
        curWeapon = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    // Метод для получения урона
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    // Метод для обработки смерти игрока
    private void Die()
    {
        Debug.Log("Player died!");
        // Логика для обработки смерти (например, перезапуск уровня)
    }
}
