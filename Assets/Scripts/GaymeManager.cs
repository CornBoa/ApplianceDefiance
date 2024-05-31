using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaymeManager : MonoBehaviour
{   
    static public GaymeManager Instance;
    public int PlayerHP;
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
    public void TakeDMG(int DMG)
    {
        PlayerHP -= DMG;
    }
    void Update()
    {
        
    }
}
