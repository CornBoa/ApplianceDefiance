using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Feeder : MonoBehaviour,ISentry
{
    float FeedCooldown = 0;
    public float feedrate = 0;
    public float feedAmount;
    public float feedRadius;
    int HP = 10;
    public GameObject RangeVis;
    public List<GameObject> meshesObjects;
    bool activated;
    void Start()
    {
        RangeVis.transform.localScale = new Vector3(feedRadius, 1, feedRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (Time.time > FeedCooldown)
            {
                FeedSentries();
                FeedCooldown = Time.time + feedrate;
            }
        }
       
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
    }

    public void Feed(float foodAmount)
    {
        throw new System.NotImplementedException();
    }
}
