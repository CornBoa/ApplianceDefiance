using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BioGenerator : MonoBehaviour,ISentry
{
    public float resupRate;
    public int resupAmount;
    public List<GameObject> meshesObjects;
    public int price;
    int HP = 5;
    bool activated;
    Slider slider;
    public void Activate()
    {
        activated = true;
        GetComponentInChildren<ParticleSystem>().Play();
        InvokeRepeating("GenerateBioMass", resupRate, resupRate);
    }
    void GenerateBioMass() 
    {
        BuildingManager.Instance.bioMaterial += resupAmount;
    }
    public void Feed(float foodAmount)
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGO()
    {
       return gameObject;
    }

    public List<GameObject> GetMeshes()
    {
        return meshesObjects;
    }

    public GameObject GetRangeVisual()
    {
        return null;
    }

    public void SpendCredit()
    {
        BuildingManager.Instance.techMaterial -= price;
    }

    public bool EnoughMaterial()
    {
        return BuildingManager.Instance.techMaterial >= price;
    }

    public void TakeDMG(int DMG)
    {
        HP -= DMG;
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.maxValue =resupRate;
        slider.value = resupRate;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated)
        {
            if (slider.value <= resupRate && slider.value != 0)
            {
                slider.value -= Time.deltaTime;
            }
            else
            {
                slider.value = resupRate;
            }
        }

    }

    public void WalkTo(Transform nodeTransform)
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}
