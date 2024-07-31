using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyTest : MonoBehaviour , IEnemy
{
    public float HP = 10;
    public List<Transform> waypoints;
    Transform currentWaypoint;
    public bool facingTarget = false,reachedTarget = false;
    public float damping =2;
    int i = 0;
    public float speed;
    public int HouseDMG, SentryDMG;
    bool ded = false;
    private ParticleSystem gothit;
    public int materialReward = 0;
    public bool ToAttack;
    public void TakeDMG(int DMG)
    {
        HP -= DMG;
        gothit.Play();
        if(HP <= 0) 
        {
            if (!ded)
            {
                WaveSpawner.Instance.EnemyDied();
                BuildingManager.Instance.techMaterial += materialReward;
            }
            ded = true;     
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gothit = GetComponentInChildren<ParticleSystem>();
        if (ToAttack) InvokeRepeating("DealSentryDMG", 0f, 2f);
    }
    void Update()
    {
       if(waypoints.Count > 0)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) <= 0.3f)
            {
                if (currentWaypoint == waypoints[waypoints.Count - 1])
                {
                    DealHouseDMG(HouseDMG);
                }
                else GetNextPoint();
            }
            if (Vector3.Distance(transform.position, currentWaypoint.position) >= 0.3f) reachedTarget = false;
            if (!reachedTarget && facingTarget)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if (!reachedTarget && !facingTarget)
            {
                var lookPos = currentWaypoint.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, damping);
                StartCoroutine(WaitTillFacing(Quaternion.Lerp(transform.rotation, rotation, 1)));
            }
        }     
    }
    void GetNextPoint()
    {
        i++;
        currentWaypoint = waypoints[i];
        facingTarget = false;
        reachedTarget = true;
    }
    IEnumerator WaitTillFacing(Quaternion rot)
    {
        yield return new WaitUntil(() => transform.rotation == rot);
        facingTarget = true;
    }

    public void SetLayout(MapLayout map)
    {
        waypoints = map.ReturnList();
        currentWaypoint = waypoints[i];
    }

    public void DealSentryDMG()
    {
        Debug.Log("DMG called");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if(collider.GetComponent<ISentry>() != null) collider.GetComponent<ISentry>().TakeDMG(SentryDMG);
            }
            Debug.Log("DMG dealt");
        }    
    }

    public void DealHouseDMG(int DMG)
    {
        GaymeManager.Instance.TakeDMG(DMG);
        if (!ded) WaveSpawner.Instance.EnemyDied(); 
        ded = true;
        Destroy(gameObject);
    }
    public GameObject GetGO()
    {
        return gameObject;
    }
}
