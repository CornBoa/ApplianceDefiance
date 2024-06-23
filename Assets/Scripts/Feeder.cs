using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Feeder : MonoBehaviour,ISentry
{
    public float feedrate = 0;
    public float feedAmount;
    public float feedRadius;
    int HP = 10;
    public GameObject RangeVis;
    public List<GameObject> meshesObjects;
    bool activated;
    public int price = 0;
    Slider slider;
    void Start()
    {
        RangeVis.transform.localScale = new Vector3(feedRadius, 1, feedRadius);  
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = feedrate;
        slider.value = feedrate;
    }
    void FeedSentries()
    {
        Debug.Log("Fed");
        Collider[] colliders = Physics.OverlapSphere(transform.position, feedRadius);
        if(colliders.Length > 0 )
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Sentry"))
                {
                    collider.GetComponent<ISentry>().Feed(feedAmount);
                }
                
            }
        }  
    }
    private void Update()
    {
        if ( activated ) 
        {
            if( slider.value <= feedrate && slider.value != 0) 
            {
                slider.value -= Time.deltaTime;
            }
            else
            {
                Debug.Log("I will shit in your head");
                slider.value = feedrate;
            }
        }

    }

    public List<GameObject> GetMeshes()
    {
        return meshesObjects;
    }

    public GameObject GetGO()
    {
        return gameObject;
    }

    public GameObject GetRangeVisual()
    {
        return RangeVis;
    }

    public void TakeDMG(int DMG)
    {
        HP -= DMG;
    }

    public void Activate()
    {
        activated = true;
        GetComponentInChildren<ParticleSystem>().Play();
        InvokeRepeating("FeedSentries", feedrate, feedrate);
    }

    public void Feed(float foodAmount)
    {
        throw new System.NotImplementedException();
    }

    public void SpendCredit()
    {
        BuildingManager.Instance.techMaterial -= price;
    }

    public bool EnoughMaterial()
    {
        return BuildingManager.Instance.techMaterial >= price; 
    }
}
