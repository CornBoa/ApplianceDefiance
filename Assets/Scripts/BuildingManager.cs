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
    public static int Material;
    public ISentry currentSentry;
    public NodeTest currentNode;
    public bool buildMode = true;
    int buildingIndex = 0;
    public int bioMaterial = 0;
    public int techMaterial = 0;
    public TextMeshProUGUI BioText,TechText;
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
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            currentSentry = null;
            buildMode = false;
        }
        BioText.text = bioMaterial.ToString();
        TechText.text = techMaterial.ToString();
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
        currentSentry.SpendCredit();
    }
    public bool IsEnoughMoney()
    {
        if (currentSentry.EnoughMaterial())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
