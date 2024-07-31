using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    MapLayout currentLayout;
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
        currentLayout = layouts[0];
        spawnPosition = currentLayout.transformList[0];
    }

    public void StartNextWave()
    {
        if (!waveInProgress && currentWaveIndex < waves.Length)
        {
            Debug.Log("Start NExt Wave Called");
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
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
        

        // Move to the next wave
        currentWaveIndex++;
        waveInProgress = false;
        wave.onWaveEnd.Invoke();
        // Optionally, wait for timeBetweenWaves before allowing the next wave to start
        // yield return new WaitForSeconds(timeBetweenWaves);
    }

    IEnumerator SpawnEnemyGroup(EnemyGroup group)
    {
        Debug.Log("SpawnEnemyGroup called");
        for (int i = 0; i < group.amount; i++)
        {
            // Spawn enemy at the position of the WaveSpawner
            IEnemy enemy = Instantiate(group.enemyPrefab, spawnPosition.position, Quaternion.identity).GetComponent<IEnemy>();
            enemy.SetLayout(currentLayout);
            enemiesAlive++;
            yield return new WaitForSeconds(group.interval);
        }
    }
    public void EnemyDied()
    {
        enemiesAlive--;
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