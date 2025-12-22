using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRadius = 10f;
    public float minSpawnDistance = 3f;
    public float spawnDelay = 2f;

    [Header("Wave Settings")]
    public int baseEnemyCount = 3;
    public int enemyIncreasePerWave = 2;

    private int waveNumber = 1;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (!GameManager.Instance.isGameOver)
        {
            UIManager.Instance.UpdateWaveUI(waveNumber);
            yield return new WaitForSeconds(2f);

            int enemiesToSpawn = baseEnemyCount + (waveNumber - 1) * enemyIncreasePerWave;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            waveNumber++;
            spawnDelay = Mathf.Max(0.5f, spawnDelay - 0.2f);

            EnemyPoolManager.Instance.ExpandPool(3);

            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        spawnPos.y = 0;

        bool isArmored = Random.value > 0.7f;
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(isArmored);

        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, spawnRadius);

        return new Vector3(
            randomCircle.x * distance,
            0,
            randomCircle.y * distance
        );
    }
}
