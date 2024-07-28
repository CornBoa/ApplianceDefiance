using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TeethSentryOne : MonoBehaviour , ISentry
{   
    public static float StatRange = 15;
    public float UsableRange;
    public Transform Target,RotatingPiece;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    public static float maxHP = 10;
    public float currentHP = 0;
    public float hunger = 10;
    public float hungerModifier;
    public GameObject Projectile;
    float nextFireTime;
    public float fireRate;
    public bool active;
    public AudioClip ShootSound;
    AudioSource ShootSource;
    public int price = 0;
    private void Start()
    {
        SphereCollider = GetComponent<SphereCollider>();
        UsableRange = StatRange;
        RangeVis.transform.localScale = new Vector3(UsableRange, 1, UsableRange);
        SphereCollider.radius = UsableRange / 2;
        currentHP = maxHP;
        ShootSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
       
        if (active) 
        {
            if (SphereCollider.radius != UsableRange / 2)
            {
                RangeVis.transform.localScale = new Vector3(UsableRange, 1, UsableRange);
                SphereCollider.radius = UsableRange / 2;
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
               if (EnemyQueue.Count > 0)
               {
                    Target = EnemyQueue[0];
               }
               else
                {
                    Target = null;
                }
            }
            if (Target != null && Vector3.Distance(transform.position, Target.position) > UsableRange/2)
            {
                Target = null;
                EnemyQueue.Remove(Target);
                if (EnemyQueue.Count > 0) Target = EnemyQueue[0];
            }
        }
        UsableRange = StatRange;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyQueue.Add(other.transform);
            }
            if (EnemyQueue.Count > 0 && EnemyQueue[0] != null) Target = EnemyQueue[0];
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        if(other == Target)Target = null;
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
        currentHP -= DMG;
    }
    void Shoot()
    {
        FleshProjectile projectile = Instantiate(Projectile, transform.position, Quaternion.identity).GetComponent<FleshProjectile>();
        projectile.Target = Target;
        ShootSource.pitch = Random.Range(0.95f, 1.1f);
        ShootSource.PlayOneShot(ShootSound);
        hunger -= 1 * hungerModifier;
    }

    public void Activate()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        active = true;
    }
    public void Feed(float foodAmount)
    {
        hunger += foodAmount;
        if (currentHP < maxHP)currentHP++;
    }
    public void SpendCredit()
    {
        BuildingManager.Instance.bioMaterial -= price;
    }

    public bool EnoughMaterial()
    {
        return BuildingManager.Instance.bioMaterial >= price;
    }

    public void WalkTo(Transform nodeTransform)
    {
        throw new System.NotImplementedException();
    }
}
