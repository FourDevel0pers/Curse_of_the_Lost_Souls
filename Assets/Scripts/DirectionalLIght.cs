using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public float dayDuration = 60f; // ѕродолжительность полного цикла (день+ночь) в секундах

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        float cycleProgress = (time % dayDuration) / dayDuration; // ќт 0 до 1
        float sunAngle = cycleProgress * 360f - 90f; // -90 чтобы солнце начинало с горизонта

        // ¬ращаем солнце
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0);

        // ћен€ем интенсивность света
        if (sunAngle > 0 && sunAngle < 180)
        {
            sun.intensity = Mathf.Lerp(0, 1f, Mathf.InverseLerp(0, 90, sunAngle)); // день
        }
        else
        {
            sun.intensity = Mathf.Lerp(1f, 0, Mathf.InverseLerp(180, 270, sunAngle)); // ночь
        }
    }
}
