using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ElectroSentry : MonoBehaviour, ISentry
{
    public static float StatRange = 15;
    public float UsableRange;
    public Transform Target, RotatingPiece;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    public static float maxHP = 10;
    public float currentHP = 0;
    public float hunger = 10;
    public float hungerModifier;
    float nextFireTime;
    public float fireRate;
    public bool active;
    public int price = 0;
    AudioSource audioSource;
    public AudioClip shoot;
    public List<IEnemy> shocked = new List<IEnemy>();
    public LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SphereCollider = GetComponent<SphereCollider>();
        RangeVis.transform.localScale = new Vector3(UsableRange, 1, UsableRange);
        SphereCollider.radius = UsableRange / 2;
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
    }
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
            if (Time.time >= nextFireTime && Target != null)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
            if (Target == null)
            {
                for (int i = 0; i < EnemyQueue.Count; i++)
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
                    audioSource.Stop();
                }
            }
            if (Target != null && Vector3.Distance(transform.position, Target.position) > UsableRange / 2)
            {
                Target = null;
                EnemyQueue.Remove(Target);
                if (EnemyQueue.Count > 0) Target = EnemyQueue[0];
            }
        }
    }
    public void Activate()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        active = true;
    }

    public void Feed(float foodAmount)
    {
        hunger += foodAmount;
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
        return RangeVis;
    }

    public void TakeDMG(int DMG)
    {
        currentHP -= DMG;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyQueue.Add(other.transform);
            }
            if (EnemyQueue.Count > 0) Target = EnemyQueue[0];
        }

    }
    public void OnTriggerExit(Collider other)
    {
        EnemyQueue.Remove(other.transform);
        if (EnemyQueue.Count > 0) Target = EnemyQueue[0];
    }
    void Shoot()
    {
        Debug.Log("Piss");
        Target.GetComponent<IEnemy>().GetElectrocuted(shocked,5,this);
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

