using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject standardEnemyPrefab;
    public GameObject armoredEnemyPrefab;

    public float spawnRadius = 10f;
    public float minSpawnDistance = 3f;
    public float spawnDelay = 2f;
    private int waveNumber = 1;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (!GameManager.Instance.isGameOver)
        {
            int enemyCount = waveNumber + 4;

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            waveNumber++;
            UIManager.Instance.UpdateWaveUI(waveNumber);
            yield return new WaitForSeconds(2f);
            spawnDelay = Mathf.Max(0.5f, spawnDelay - 0.2f);
            yield return new WaitForSeconds(2f);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        spawnPos.y = 0;

        GameObject enemyPrefab =
            Random.value > 0.7f ? armoredEnemyPrefab : standardEnemyPrefab;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minSpawnDistance, spawnRadius);

        return new Vector3(randomCircle.x * distance, 0, randomCircle.y * distance);
    }
}
