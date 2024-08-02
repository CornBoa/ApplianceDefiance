using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    public bool Volume, Sens,Music,TimeScale;
    Slider THis;
    private void Start()
    {
        THis = GetComponent<Slider>();
        if (Volume && !Sens) THis.value = SavedValues.Volume;
        else if (Music) THis.value = SavedValues.MusicVolume;
    }
    public void SetVolume()
    {
        SavedValues.Volume = THis.value;
    }
    public void SetMusicVolume()
    {
        SavedValues.MusicVolume = THis.value;
    }
    public void SetTimescale()
    {
        Time.timeScale = THis.value;
    }
}
