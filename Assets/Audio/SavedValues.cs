using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SavedValues : MonoBehaviour
{
    public static float Volume = -30;
    public static float MusicVolume = -30;
    public static int WaveIndex;
    public static bool HasASword = false, FirstRun = true, TalkedToGolem = false;
    public AudioMixer mixer;
    public float VisAudio;
    public static int TechMat, BioMat;
    void Start()
    {
        DontDestroyOnLoad(gameObject);       
    }
    void Update()
    {        
        VisAudio = Volume;
        mixer.SetFloat("Volume", Volume);
        mixer.SetFloat("Music", MusicVolume);
    }
    public void VoidSetWave()
    {
        FindObjectOfType<WaveSpawner>().currentWaveIndex = WaveIndex;
    }
}
