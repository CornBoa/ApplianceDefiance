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
    public int HouseDMG, SentryGroundDMG,SentrySpinDMG;
    bool ded = false;
    private ParticleSystem gothit;
    public int materialReward = 0;
    public bool AttackIng;
    public UnityEvent OnHouseReached, OnDeath;
    public float AttackTimer;
    public GameObject LeftHand, RightHand,RocketPrefab;
    public bool LeftRightHAnd;
    Animator animator;
    public ParticleSystem BoomSystem;    
   
    public void TakeDMG(int DMG)
    {
        HP -= DMG;
        gothit.Play();
        BuildingManager.Instance.techMaterial += materialReward;
        if (HP <= 0)
        {
            if (!ded)
            {
                AttackIng = true;
                WaveSpawner.Instance.EnemyDied();
                ded = true;              
                animator.gameObject.SetActive(false);
                BoomSystem.Emit(1);                
                StartCoroutine(WaitAfterDeath());
            }      
        }
    }
    void Start()
    {
       
        gothit = GetComponentInChildren<ParticleSystem>();
        InvokeRepeating("PickAttack", 2f, AttackTimer);
        animator = GetComponentInChildren<Animator>();
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
    void PickAttack()
    {
        int whichAttack = Random.Range(0, 3);
        switch (whichAttack)
        {
            case 0:
                animator.SetTrigger("Spin");
                Debug.Log("Attack 1 called");
                break;
            case 1:
                Debug.Log("Attack 2 called");
                animator.SetTrigger("Slam");
                break;
            case 2:
                Debug.Log("Attack 3 called");
                animator.SetTrigger("Shoot");
                break;

        }
    }
    public void AttackOne()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<ISentry>() != null) collider.GetComponent<ISentry>().TakeDMG(SentrySpinDMG);
            }
            Debug.Log("DMG dealt");
        }
    }
    public void AttackTwo()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<ISentry>() != null) collider.GetComponent<ISentry>().TakeDMG(SentryGroundDMG);
            }
            Debug.Log("DMG dealt");
        }
    }
    public void AttackThree()
    {
        if (LeftRightHAnd)
        {
            Instantiate(RocketPrefab,RightHand.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(RocketPrefab, LeftHand.transform.position, Quaternion.identity);
        }
    }

    public void DealHouseDMG(int DMG)
    {
        OnHouseReached.Invoke();
    }
    public GameObject GetGO()
    {
        return gameObject;
    }
    IEnumerator WaitAfterDeath()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("coroutine finished");              
        FindObjectOfType<DeathHandler>().Won();
       // Destroy(gameObject);
    }
    public void DealSentryDMG()
    {
        throw new System.NotImplementedException();
    }
}
