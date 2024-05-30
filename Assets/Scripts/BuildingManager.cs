using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    public List<GameObject> publicSentries;
    List<ISentry> listOfSentries = new List<ISentry>();
    public static int Material;
    public ISentry currentSentry;
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
    }
    public void SetSentry(int index)
    {
        currentSentry = listOfSentries[index];
    }
    public void SentryUnset()
    {
        currentSentry = null;
    }
}
