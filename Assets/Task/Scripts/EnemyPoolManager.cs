using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [SerializeField] private GameObject standardEnemyPrefab;
    [SerializeField] private GameObject armoredEnemyPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int poolGrowthPerWave = 5;

    private Queue<GameObject> standardPool = new Queue<GameObject>();
    private Queue<GameObject> armoredPool = new Queue<GameObject>();

    private int activeStandard;
    private int activeArmored;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PreAllocatePool(initialPoolSize);
    }

    private void PreAllocatePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject standard = Instantiate(standardEnemyPrefab, transform);
            standard.SetActive(false);
            standardPool.Enqueue(standard);

            GameObject armored = Instantiate(armoredEnemyPrefab, transform);
            armored.SetActive(false);
            armoredPool.Enqueue(armored);
        }
    }

    public void ExpandPool(int additionalCount)
    {
        PreAllocatePool(additionalCount);
    }

    public GameObject GetEnemy(bool isArmored)
    {
        Queue<GameObject> pool = isArmored ? armoredPool : standardPool;
        GameObject prefab = isArmored ? armoredEnemyPrefab : standardEnemyPrefab;

        GameObject enemy;

        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            enemy = Instantiate(prefab, transform);
        }

        enemy.SetActive(true);

        if (isArmored) activeArmored++;
        else activeStandard++;

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy, bool isArmored)
    {
        enemy.SetActive(false);

        if (isArmored)
        {
            armoredPool.Enqueue(enemy);
            activeArmored--;
        }
        else
        {
            standardPool.Enqueue(enemy);
            activeStandard--;
        }
    }

    public int ActiveEnemyCount()
    {
        return activeStandard + activeArmored;
    }
}
