using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public Wave currentWave;
    public List<Wave> waves;
    Group currentGroup;
    private float[] nextSpawnTimes;
    public Transform spawnPosition;
    public List<MapLayout> layouts;
    MapLayout currentLayout;
    public float timeBetweenWaves;
    int waveIndex = 0;
    int groupIndex = 0;
    public bool WaveGoing = false;
    void Start()
    {
        currentLayout = layouts[0];
        currentWave = waves[waveIndex];
        currentGroup = waves[waveIndex].groupsOfWave[groupIndex];
        nextSpawnTimes = new float[currentGroup.enemyPrefabs.Count];
        for (int i = 0; i < nextSpawnTimes.Length; i++)
        {
            nextSpawnTimes[i] = Time.time + currentGroup.spawnIntervals[i];
        }
        spawnPosition = currentLayout.transformList[0];
    }
    void Update()
    {
        if (WaveGoing)
        {
            for (int i = 0; i < currentGroup.enemyPrefabs.Count; i++)
            {
                if (Time.time >= nextSpawnTimes[i])
                {
                    SpawnEnemy(i);
                    nextSpawnTimes[i] = Time.time + currentGroup.spawnIntervals[i];
                }
            }
        }
    }
    public IEnumerator TimeBetweenGroups()
    {
           if (groupIndex < waves[waveIndex].groupsOfWave.Length-1)
           {
                yield return new WaitForSeconds(waves[waveIndex].groupsOfWave[groupIndex].timeBeforeNextGroup);
                groupIndex++;
                currentGroup = waves[waveIndex].groupsOfWave[groupIndex];
           }
           else
           {
                WaveGoing = false;
                Debug.Log("OnWaveEndCalled");
                currentWave.OnWaveEnd.Invoke();
           }       
    }
    public void StartWaveEnum()
    {
        Debug.Log("WaveCooldownStarted");
        StartCoroutine(StartNextWave());
    }
    public IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        waveIndex++;
        groupIndex = 0;
        currentWave = waves[waveIndex];
        currentGroup = waves[waveIndex].groupsOfWave[groupIndex];
        WaveGoing = true;

    }
    void SpawnEnemy(int index)
    {
        if (currentGroup.spawnAmount[index] > 0)
        {
            IEnemy enemy = Instantiate(currentGroup.enemyPrefabs[index], spawnPosition.position, Quaternion.identity).GetComponent<IEnemy>();
            enemy.SetLayout(currentLayout);
            currentGroup.spawnAmount[index]--;
        }   
        else if(currentGroup.spawnAmount[index] == 0)
        {
            currentGroup.enemyPrefabs.RemoveAt(index);
            StartCoroutine(TimeBetweenGroups());
        }
    }
}
[System.Serializable]
public class Wave
{
    public Group[] groupsOfWave;
    public UnityEvent OnWaveEnd;
    void StartWave()
    {
        foreach (var group in groupsOfWave)
        {
            group.mamaWave = this;
        }
    }
}

[System.Serializable]
public class Group
{
    public List<GameObject> enemyPrefabs;
    public float[] spawnIntervals;
    public int[] spawnAmount;
    public float timeBeforeNextGroup;
    public Wave mamaWave;
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