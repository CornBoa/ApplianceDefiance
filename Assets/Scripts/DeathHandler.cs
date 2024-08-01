using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DeathHandler : MonoBehaviour
{
    public UnityEvent OnDeath,OnRestart,OnWin;
    public GameObject deathScreen,winScreen;   
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void Died()
    {
        foreach (NodeTest node in FindObjectsOfType<NodeTest>())
        {
            node.occupied = false;
            if(node.sentryInstalled != null)Destroy(node.sentryInstalled.GetGO());
            node.sentryInstalled = null;
        }
        foreach (EnemyTest enemy in FindObjectsOfType<EnemyTest>())
        {
            Destroy(enemy.gameObject);
        }
        OnDeath.Invoke();
        deathScreen.SetActive(true);
        SavedValues.WaveIndex = FindObjectOfType<WaveSpawner>().currentWaveIndex;
        GaymeManager.Instance.PlayerHP = 100;
    }
    public void Reset()
    {
        OnRestart?.Invoke();
        deathScreen?.SetActive(false);
        FindObjectOfType<WaveSpawner>().currentWaveIndex = SavedValues.WaveIndex;
        BuildingManager.Instance.bioMaterial = SavedValues.BioMat;
        BuildingManager.Instance.techMaterial = SavedValues.TechMat;
        WaveSpawner.Instance.ResetWave();
        WaveSpawner.Instance.StartNextWave();
    }
    public void Won()
    {
        foreach (NodeTest node in FindObjectsOfType<NodeTest>())
        {
            node.occupied = false;
            if (node.sentryInstalled != null) Destroy(node.sentryInstalled.GetGO());
            node.sentryInstalled = null;
        }
        foreach (EnemyTest enemy in FindObjectsOfType<EnemyTest>())
        {
            Destroy(enemy.gameObject);
        }
        winScreen.SetActive(true);
        OnWin.Invoke();
    }
}
