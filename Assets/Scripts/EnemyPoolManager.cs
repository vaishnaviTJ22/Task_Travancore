using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [SerializeField] private GameObject standardEnemyPrefab;
    [SerializeField] private GameObject armoredEnemyPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 15;
    [SerializeField] private int maxPoolSize = 50;
    [SerializeField] private int expandThreshold = 3;

    private List<GameObject> standardPool = new List<GameObject>();
    private List<GameObject> armoredPool = new List<GameObject>();

    private int totalStandardCreated = 0;
    private int totalArmoredCreated = 0;
    private int activeStandard = 0;
    private int activeArmored = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PreAllocatePool(initialPoolSize);
    }

    private void PreAllocatePool(int totalCount)
    {
        int standardCount = Mathf.CeilToInt(totalCount * 0.7f);
        int armoredCount = Mathf.CeilToInt(totalCount * 0.3f);

        CreateEnemies(standardCount, armoredCount);
    }

    private void CreateEnemies(int standardCount, int armoredCount)
    {
        for (int i = 0; i < standardCount; i++)
        {
            if (totalStandardCreated >= maxPoolSize) break;

            GameObject standard = Instantiate(standardEnemyPrefab, transform);
            standard.SetActive(false);
            standardPool.Add(standard);
            totalStandardCreated++;
        }

        for (int i = 0; i < armoredCount; i++)
        {
            if (totalArmoredCreated >= maxPoolSize) break;

            GameObject armored = Instantiate(armoredEnemyPrefab, transform);
            armored.SetActive(false);
            armoredPool.Add(armored);
            totalArmoredCreated++;
        }
    }

    public GameObject GetEnemy(bool isArmored)
    {
        List<GameObject> pool = isArmored ? armoredPool : standardPool;
        GameObject prefab = isArmored ? armoredEnemyPrefab : standardEnemyPrefab;

        GameObject enemy = null;

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                enemy = pool[i];
                break;
            }
        }

        if (enemy == null)
        {
            int currentTotal = isArmored ? totalArmoredCreated : totalStandardCreated;

            if (currentTotal < maxPoolSize)
            {
                enemy = Instantiate(prefab, transform);
                pool.Add(enemy);

                if (isArmored) totalArmoredCreated++;
                else totalStandardCreated++;

                Debug.LogWarning($"Pool expanded on-demand! {(isArmored ? "Armored" : "Standard")} pool size: {pool.Count}");
            }
            else
            {
                Debug.LogError("Max pool size reached! Cannot spawn more enemies.");
                return null;
            }
        }

        enemy.SetActive(true);

        if (isArmored) activeArmored++;
        else activeStandard++;

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy, bool isArmored)
    {
        if (enemy == null) return;

        enemy.SetActive(false);

        if (isArmored) activeArmored--;
        else activeStandard--;
    }

    public void OptimizePoolForWave(int waveNumber, int enemiesToSpawn)
    {
        int standardNeeded = Mathf.CeilToInt(enemiesToSpawn * 0.7f);
        int armoredNeeded = Mathf.CeilToInt(enemiesToSpawn * 0.3f);

        int availableStandard = GetAvailableCount(standardPool);
        int availableArmored = GetAvailableCount(armoredPool);

        int standardToCreate = Mathf.Max(0, standardNeeded - availableStandard);
        int armoredToCreate = Mathf.Max(0, armoredNeeded - availableArmored);

        if (standardToCreate > 0 || armoredToCreate > 0)
        {
            CreateEnemies(standardToCreate, armoredToCreate);
        }
    }

    private int GetAvailableCount(List<GameObject> pool)
    {
        int count = 0;
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                count++;
        }
        return count;
    }

    public int ActiveEnemyCount()
    {
        return activeStandard + activeArmored;
    }

    public int GetTotalPoolSize()
    {
        return totalStandardCreated + totalArmoredCreated;
    }

    public void LogPoolStats()
    {
        Debug.Log($"Pool Stats - Standard: {totalStandardCreated} (Active: {activeStandard}), Armored: {totalArmoredCreated} (Active: {activeArmored})");
    }
}
