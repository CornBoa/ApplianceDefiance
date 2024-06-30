using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public RectTransform panel;   
    public Image arrows;
    public Sprite arrowup;
    public Sprite arrowdown;
    public bool active;
    void Start()
    {
        active = false;
    }

    
    void Update()
    {
        if (active) 
        {
            panel.anchoredPosition = new Vector2(0, 150);
            arrows.sprite = arrowdown;
            
        }
        else
        { 
            panel.anchoredPosition = new Vector2(0, 0); 
            arrows.sprite = arrowup;
        }
            
    }
    public void ChangeBool()
    {
        if (!active)
        { active = true; }
        else 
        { active = false; }         

    }
}
