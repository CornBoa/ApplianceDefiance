using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethSentry : MonoBehaviour , ISentry
{   
    private float Range;
    public float PublicRange;
    public Transform Target,RotatingPiece;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    float HP = 10;
    public float hunger = 10;
    public float hungerModifier;
    public GameObject Projectile;
    float nextFireTime;
    public float fireRate;
    public bool active;
    private void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        Range = PublicRange;
        RangeVis.transform.localScale = new Vector3(PublicRange, 1, PublicRange);
        SphereCollider.radius = Range / 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(active) 
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
            if (Time.time >= nextFireTime && hunger > 0 && Target != null)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
            if (Target == null)
            {
               for(int i = 0;i < EnemyQueue.Count;i++) 
                {
                    if (EnemyQueue[i] == null)
                    {
                        EnemyQueue.RemoveAt(i);
                    }
                }
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyQueue.Add(other.transform);
            }
            if (EnemyQueue[0] != null) Target = EnemyQueue[0];
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        EnemyQueue.Remove(other.transform);
        if (EnemyQueue.Count > 0) Target = EnemyQueue[0];
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
    void Shoot()
    {
        FleshProjectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<FleshProjectile>();
        projectile.Target = Target;
        hunger -= 1 * hungerModifier;
    }

    public void Activate()
    {
        active = true;
    }
    public void Feed(float foodAmount)
    {
        hunger += foodAmount;
    }
}
