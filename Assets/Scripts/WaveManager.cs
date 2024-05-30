using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    Wave currentWave;
    private float[] nextSpawnTimes;
    public Transform spawnPosition;
    float minX, maxX;
    public List<MapLayout> layouts;
    MapLayout currentLayout;
    void Start()
    {
        currentLayout = layouts[0];
       currentWave = waves[0];
        nextSpawnTimes = new float[currentWave.enemyPrefabs.Length];
        for (int i = 0; i < nextSpawnTimes.Length; i++)
        {
            nextSpawnTimes[i] = Time.time + currentWave.spawnIntervals[i];
        }
        spawnPosition = currentLayout.transformList[0];
    }
    void Update()
    {
        for (int i = 0; i < currentWave.enemyPrefabs.Length; i++)
        {
            if (Time.time >= nextSpawnTimes[i])
            {
                SpawnEnemy(i);
                nextSpawnTimes[i] = Time.time + currentWave.spawnIntervals[i];
            }
        }
    }

    void SpawnEnemy(int index)
    {
        if (currentWave.spawnAmount[index] > 0)
        {
            IEnemy enemy = Instantiate(currentWave.enemyPrefabs[index], spawnPosition.position, Quaternion.identity).GetComponent<IEnemy>();
            enemy.SetLayout(currentLayout);
            currentWave.spawnAmount[index]--;
        }   
    }
}
[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefabs;
    public float[] spawnIntervals;
    public int[] spawnAmount;
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