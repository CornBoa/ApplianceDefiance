using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour , IEnemy
{
    public float HP = 200;
    public List<Transform> waypoints;
    Transform currentWaypoint;
    public bool facingTarget = false, reachedTarget = false;
    public float damping = 2;
    int i = 0;
    public float speed;
    public int HouseDMG, SentryDMG;
    WaveManager Mamah;
    bool ded = false;
    private ParticleSystem gothit;
    public int materialReward = 0;
    public bool AttackIng;
    public UnityEvent OnHouseReached, OnDeath;
    float AttackTimer;
    public float fireRate;
    public void TakeDMG(int DMG)
    {
        HP -= DMG;
        gothit.Play();
        BuildingManager.Instance.techMaterial += materialReward;
        if (HP <= 0)
        {
            if (!ded)
            {
                Mamah.enemiesSpawmned--;
                ded = true;
                OnDeath.Invoke();
            }      
        }
    }
    void Start()
    {
        gothit = GetComponentInChildren<ParticleSystem>();
        Mamah = GameObject.FindAnyObjectByType<WaveManager>();
    }
    void Update()
    {
        if (waypoints.Count > 0)
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
            if (!reachedTarget && facingTarget && !AttackIng)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if (!reachedTarget && !facingTarget && !AttackIng)
            {
                var lookPos = currentWaypoint.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, damping);
                StartCoroutine(WaitTillFacing(Quaternion.Lerp(transform.rotation, rotation, 1)));
            }
            if (Time.time >= AttackTimer)
            {
                AttackTimer = Time.time + fireRate;
                int whichAttack = Random.Range(0, 2);
                switch(whichAttack)
                {
                    case 0:
                        AttackOne();
                        Debug.Log("Attack 1 called");
                        break;
                    case 1:
                        Debug.Log("Attack 2 called");
                        AttackTwo();
                        break;
                    case 2:
                        Debug.Log("Attack 3 called");
                        AttackThree();
                        break;

                }
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

    public void AttackOne()
    {

    }
    public void AttackTwo()
    {

    }
    public void AttackThree()
    {

    }

    public void DealHouseDMG(int DMG)
    {
        OnHouseReached.Invoke();
    }
    public GameObject GetGO()
    {
        return gameObject;
    }

    public void DealSentryDMG()
    {
        throw new System.NotImplementedException();
    }
}
