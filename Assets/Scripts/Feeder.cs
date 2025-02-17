using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Feeder : MonoBehaviour,ISentry
{
    public float feedrate = 0;
    public float feedAmount;
    public float feedRadius;
    int HP = 10;
    Transform nodePosition;
    public GameObject RangeVis;
    public List<GameObject> meshesObjects;
    bool activated, walk;
    public int price = 0;
    Slider slider;
    NodeTest myNode;
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
            if (walk)
            {
                transform.LookAt(nodePosition.position);
                transform.position = Vector3.MoveTowards(transform.position, nodePosition.position, 1f);
                if (Vector3.Distance(transform.position, nodePosition.position) < 1)
                {
                    walk = false;
                    Activate();
                }
            }
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
        if (HP <= 0) Die();
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

    public void WalkTo(Transform nodeTransform)
    {
        nodePosition = nodeTransform;
        walk = true;
    }

    public void Die()
    {
        myNode.occupied = false;
        Destroy(gameObject);

    }

    public void CashBack()
    {
        BuildingManager.Instance.techMaterial += Convert.ToInt32(price * 0.25f);
    }

    public void MyNode(NodeTest node)
    {
        myNode = node;
    }
}
