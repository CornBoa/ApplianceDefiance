using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaymeManager : MonoBehaviour
{   
    static public GaymeManager Instance;
    public int PlayerHP;
    public Slider PlayerHPBar;
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
        PlayerHPBar.maxValue = PlayerHP;
    }
    public void TakeDMG(int DMG)
    {
        PlayerHP -= DMG;
        if (PlayerHP <= 0)
        {
            FindAnyObjectByType<DeathHandler>().Died();
        }

    }
    void Update()
    {
        PlayerHPBar.value = PlayerHP;

    }
}
