using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    public List<GameObject> publicSentries;
    List<ISentry> listOfSentries = new List<ISentry>();
    public List<int> Prices = new List<int>();
    public static int Material;
    public ISentry currentSentry;
    public NodeTest currentNode;
    public bool buildMode = true;
    int buildingIndex = 0;
    public int bioMaterial = 0;
    public TextMeshProUGUI MunehTextTempor;
    public AudioClip PickSound;
    AudioSource PickSource;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (var entry in publicSentries)
        {
            ISentry sentry = entry.GetComponent<ISentry>();
            if(sentry != null) listOfSentries.Add(sentry);
        }
        PickSource = GetComponent<AudioSource>();
        MunehTextTempor.text = bioMaterial.ToString();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            currentSentry = null;
            buildMode = false;
        }

    }
    public void EngageBuildMode()
    {
        buildMode = true;
    }
    public void SetSentry(int index)
    {
        PickSource.PlayOneShot(PickSound);
        buildMode = true;
        currentSentry = listOfSentries[index];
    }
    public void SentryUnset()
    {
        currentSentry = null;
    }
    public void MoneySpend()
    {
        bioMaterial -= Prices[buildingIndex];
        MunehTextTempor.text = bioMaterial.ToString();
    }
    public bool IsEnoughMoney()
    {
        if (bioMaterial >= Prices[buildingIndex])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
