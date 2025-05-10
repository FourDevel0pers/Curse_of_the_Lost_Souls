using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public float dayDuration = 60f; // ����������������� ������� ����� (����+����) � ��������

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        float cycleProgress = (time % dayDuration) / dayDuration; // �� 0 �� 1
        float sunAngle = cycleProgress * 360f - 90f; // -90 ����� ������ �������� � ���������

        // ������� ������
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0);

        // ������ ������������� �����
        if (sunAngle > 0 && sunAngle < 180)
        {
            sun.intensity = Mathf.Lerp(0, 1f, Mathf.InverseLerp(0, 90, sunAngle)); // ����
        }
        else
        {
            sun.intensity = Mathf.Lerp(1f, 0, Mathf.InverseLerp(180, 270, sunAngle)); // ����
        }
    }
}
