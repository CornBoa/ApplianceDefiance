using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTest : MonoBehaviour , ISentry
{   
    private float Range;
    [Range(0, 15)]
    public float PublicRange;
    public Transform Target,RotatingPiece;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    float HP = 10;
    void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        Range = PublicRange;
        RangeVis.transform.localScale = new Vector3(PublicRange, 1, PublicRange);
        SphereCollider.radius = Range / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (PublicRange != Range)
        {
            Range = PublicRange;
            RangeVis.transform.localScale = new Vector3(PublicRange, 1, PublicRange);
            SphereCollider.radius = Range / 2;
        }
        if (EnemyQueue.Count != 0 && Target != null) 
        {
            Vector3 dir = Target.position - transform.position;
            Quaternion lookDirection = Quaternion.LookRotation(dir);
            Vector3 rotation = lookDirection.eulerAngles;
            RotatingPiece.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyQueue.Add(other.transform);    
        }
        if (EnemyQueue[0] != null) Target = EnemyQueue[0];
    }
    public void OnTriggerExit(Collider other)
    {
        if (EnemyQueue.Count > 0 && other.transform == EnemyQueue[0])
        {
            EnemyQueue.RemoveAt(0);
            if (EnemyQueue.Count > 0 && EnemyQueue[0] != null)
            {
                EnemyQueue.RemoveAt(0);
                if(EnemyQueue.Count > 0) Target = EnemyQueue[0];
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

    public void TakeDMG()
    {
        throw new System.NotImplementedException();
    }
}
