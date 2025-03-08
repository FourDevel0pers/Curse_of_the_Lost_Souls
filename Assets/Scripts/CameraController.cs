using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1.0f;
    [SerializeField] private float minCameraAngle = -80f;
    [SerializeField] private float maxCameraAngle = 80f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;


    private void Update()
    {
        rotationX = Input.GetAxis("Mouse X") * sensitivity * Time.timeScale;
        rotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity * Time.timeScale;

        transform.parent.Rotate(0, rotationX, 0);

        if (rotationY > 180) rotationY -= 360;
        rotationY = Mathf.Clamp(rotationY, minCameraAngle, maxCameraAngle);
        if (rotationY < 0) rotationY += 360;

        transform.localEulerAngles = new Vector3(rotationY, 0, transform.localEulerAngles.z);
    }
}
