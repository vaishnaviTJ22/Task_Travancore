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
    public float timeBetweenWaves = 1f;

    private int waveNumber = 1;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (!GameManager.Instance.isGameOver)
        {
            if (!GameManager.Instance.isGameOver)
            {
                UIManager.Instance.UpdateWaveUI(waveNumber);
            }

            yield return new WaitForSeconds(2f);

            if (GameManager.Instance.isGameOver) break;

            int enemiesToSpawn = baseEnemyCount + (waveNumber - 1) * enemyIncreasePerWave;

            EnemyPoolManager.Instance.OptimizePoolForWave(waveNumber, enemiesToSpawn);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (GameManager.Instance.isGameOver) break;

                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            if (GameManager.Instance.isGameOver) break;

            yield return StartCoroutine(WaitForAllEnemiesDead());

            if (GameManager.Instance.isGameOver) break;

            waveNumber++;
            spawnDelay = Mathf.Max(0.5f, spawnDelay - 0.2f);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator WaitForAllEnemiesDead()
    {
        while (EnemyPoolManager.Instance.ActiveEnemyCount() > 0)
        {
            if (GameManager.Instance.isGameOver) break;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("All enemies defeated! Preparing next wave...");
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        spawnPos.y = 0;

        bool isArmored = Random.value > 0.7f;
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(isArmored);

        if (enemy == null) return;

        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetEnemyType(isArmored);
        }
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
