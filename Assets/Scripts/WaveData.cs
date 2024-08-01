using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public List<WaveSpawner.EnemyGroup> enemyGroups;

    public WaveData(WaveSpawner.Wave wave)
    {
        enemyGroups = new List<WaveSpawner.EnemyGroup>();
        foreach (var group in wave.enemyGroups)
        {
            enemyGroups.Add(new WaveSpawner.EnemyGroup
            {
                enemyPrefab = group.enemyPrefab,
                amount = group.amount,
                interval = group.interval
            });
        }
    }
}