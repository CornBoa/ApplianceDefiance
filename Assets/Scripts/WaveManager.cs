using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float[] spawnIntervals;

    private float[] nextSpawnTimes;

    float minX, maxX;
    void Start()
    {
        nextSpawnTimes = new float[enemyPrefabs.Length];
        SetBoundaries();
        for (int i = 0; i < nextSpawnTimes.Length; i++)
        {
            nextSpawnTimes[i] = Time.time + spawnIntervals[i];
        }
    }
    void Update()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (Time.time >= nextSpawnTimes[i])
            {
                SpawnEnemy(i);
                nextSpawnTimes[i] = Time.time + spawnIntervals[i];
            }
        }
    }

    void SpawnEnemy(int index)
    {
        float spawnPosX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(spawnPosX, 0, transform.position.z);
        Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity);
    }
    void SetBoundaries()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

            minX = bottomLeft.x;
            maxX = topRight.x;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }
}
