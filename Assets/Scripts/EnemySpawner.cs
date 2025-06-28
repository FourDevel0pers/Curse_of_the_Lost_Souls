using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ �����
    public float spawnInterval = 40f; // ���� ���� ������ 40 ������
    public int maxEnemies = 10;

    private int spawnedEnemies = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (spawnedEnemies >= maxEnemies) return;

        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedEnemies++;
    }
}
