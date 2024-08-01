using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public UnityEvent stuffTostart;
    public void StartThingies()
    {
       stuffTostart.Invoke();
    }
    public void QuitThisSHit()
    {
        Application.Quit();
    }
}
