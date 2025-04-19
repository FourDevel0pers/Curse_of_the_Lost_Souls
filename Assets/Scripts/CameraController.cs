using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1.0f;  // ������� ��������� ��� ��������
    public float maxYAngle = 80.0f;
    private float rotationX = 0.0f;

    private void Update()
    {
        // ������� ��� ���� �� ���� X � Y
        float mouseX = Input.GetAxis("Mouse X") * Time.timeScale;
        float mouseY = Input.GetAxis("Mouse Y") * Time.timeScale;

        // ������� ������������ ��'���� �� �� Y (����/������)
        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);

        // ������� ������ �� �� X (�����/����), ��������� ������������ ���������
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}
