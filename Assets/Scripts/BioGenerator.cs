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
    Transform nodePosition;
    bool activated, walk;
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
        if (HP <= 0) Die();
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
        if (walk)
        {
            transform.LookAt(nodePosition.position);
            transform.position = Vector3.MoveTowards(transform.position, nodePosition.position, 1f);
            if (Vector3.Distance(transform.position, nodePosition.position) < 1)
            {
                walk = false;
                activated = true;
            }
        }
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
        nodePosition = nodeTransform;
        walk = true;
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
    public void CashBack()
    {
        BuildingManager.Instance.techMaterial += Convert.ToInt32(price * 0.25f);
    }
}
