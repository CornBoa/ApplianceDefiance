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
    WaveManager Mamah;
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
                Mamah.enemiesSpawmned--;
                BuildingManager.Instance.techMaterial += materialReward;
            }
            ded = true;     
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gothit = GetComponentInChildren<ParticleSystem>();
        Mamah = GameObject.FindAnyObjectByType<WaveManager>();
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
        if (!ded) Mamah.enemiesSpawmned--;
        ded = true;
        Destroy(gameObject);
    }

    public void GetElectrocuted(List<IEnemy> shocked,int DMG,ElectroSentry sentry)
    {
        List<IEnemy> currentChain = shocked;
        currentChain.Add(this);
        if (currentChain.Count == 4)
        {
            Debug.Log("Took DMG");           
            sentry.lineRenderer.positionCount = currentChain.Count;
            List<Vector3> transformsOfEnemies = new List<Vector3>();
            foreach (IEnemy enemy in currentChain)
            {
                transformsOfEnemies.Add(enemy.GetGO().transform.position);
                enemy.TakeDMG(10);
            }
            sentry.lineRenderer.SetPositions(transformsOfEnemies.ToArray());
            sentry.shocked.Clear();
        }
        else
        {
            Debug.Log("Chain continue");
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5, gameObject.layer);
            if (colliders.Length > 0)
            {
                Collider ClosestEnemy = colliders[1];
                foreach (Collider nearbyObject in colliders)
                {
                    if (Vector3.Distance(transform.position, nearbyObject.gameObject.transform.position) < Vector3.Distance(transform.position, ClosestEnemy.gameObject.transform.position) && nearbyObject != this)
                    {
                        ClosestEnemy = nearbyObject;
                    }
                }
                ClosestEnemy.GetComponent<IEnemy>().GetElectrocuted(currentChain, DMG, sentry);
            }                    
        }
    }

    public GameObject GetGO()
    {
        return gameObject;
    }
}
