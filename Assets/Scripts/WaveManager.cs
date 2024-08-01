using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;
        public int amount;
        public float interval;
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyGroup[] enemyGroups;
        public UnityEvent onWaveEnd;
    }

    public Wave[] waves;
    public float timeBetweenWaves;

    public int currentWaveIndex = 0;
    public bool waveInProgress = false;
    private int enemiesAlive = 0;
    public static WaveSpawner Instance { get; private set; }

    public List<MapLayout> layouts;
    public Transform spawnPosition;
    private MapLayout currentLayout;

    private List<WaveData> initialWavesData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (layouts != null && layouts.Count > 0)
        {
            currentLayout = layouts[0];
            spawnPosition = currentLayout.transformList[0];
        }
        else
        {
            Debug.LogError("No layouts assigned!");
        }

        SaveInitialWaveData();
    }

    private void SaveInitialWaveData()
    {
        initialWavesData = new List<WaveData>();
        foreach (var wave in waves)
        {
            initialWavesData.Add(new WaveData(wave));
        }
    }

    public void StartNextWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            Debug.Log("StartNextWave called");
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        SavedValues.BioMat = BuildingManager.Instance.bioMaterial;
        SavedValues.TechMat = BuildingManager.Instance.techMaterial;
        waveInProgress = true;
        Debug.Log("WaveSpawned");

        // Loop through each enemy group in the wave
        foreach (EnemyGroup group in wave.enemyGroups)
        {
            yield return StartCoroutine(SpawnEnemyGroup(group));
        }

        while (enemiesAlive > 0)
        {
            yield return null;
        }

        // Trigger the onWaveEnd event
        wave.onWaveEnd.Invoke();

        // Move to the next wave
        currentWaveIndex++;
        waveInProgress = false;

        Debug.Log("Wave completed. Moving to wave index: " + currentWaveIndex);
    }

    IEnumerator SpawnEnemyGroup(EnemyGroup group)
    {
        Debug.Log("SpawnEnemyGroup called");
        for (int i = 0; i < group.amount; i++)
        {
            // Spawn enemy at the position of the WaveSpawner
            IEnemy enemy = Instantiate(group.enemyPrefab, spawnPosition.position, Quaternion.identity).GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.SetLayout(currentLayout);
                enemiesAlive++;
                Debug.Log("Spawned enemy. Enemies alive: " + enemiesAlive);
            }
            else
            {
                Debug.LogError("Enemy prefab does not have an IEnemy component!");
            }
            yield return new WaitForSeconds(group.interval);
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        Debug.Log("Enemy died. Enemies alive: " + enemiesAlive);
    }

    public void ResetWave()
    {
        if (waveInProgress)
        {
            StopAllCoroutines();
            waveInProgress = false;
            enemiesAlive = 0;

            // Reset the wave to its initial state
            var initialWave = initialWavesData[currentWaveIndex];
            waves[currentWaveIndex].enemyGroups = new EnemyGroup[initialWave.enemyGroups.Count];
            for (int i = 0; i < initialWave.enemyGroups.Count; i++)
            {
                waves[currentWaveIndex].enemyGroups[i] = new EnemyGroup
                {
                    enemyPrefab = initialWave.enemyGroups[i].enemyPrefab,
                    amount = initialWave.enemyGroups[i].amount,
                    interval = initialWave.enemyGroups[i].interval
                };
            }

            Debug.Log("Wave reset. Ready to restart the wave.");
        }
    }
}

[System.Serializable]
public class MapLayout
{
    public List<Transform> transformList;
    public List<Transform> ReturnList()
    {
        return transformList;
    }
}