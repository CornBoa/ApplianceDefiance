using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NapalmSentry : MonoBehaviour , ISentry
{
    public static float StatRange = 15;
    public float UsableRange;
    public Transform Target, RotatingPiece;
    Transform nodePosition;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    public static float maxHP = 10;
    public float currentHP = 0;
    public float hunger = 10;
    public float hungerModifier;
    public List<GameObject> Projectiles;
    float nextFireTime;
    public float fireRate;
    public bool active,walk;
    public int price = 0;
    AudioSource audioSource;
    public AudioClip shoot;
    Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        SphereCollider = GetComponent<SphereCollider>();
        UsableRange = StatRange;
        RangeVis.transform.localScale = new Vector3(UsableRange, 1, UsableRange);
        SphereCollider.radius = UsableRange / 2;
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        if (walk)
        {
            animator.SetBool("Walk", walk);
            transform.LookAt(nodePosition.position);
            transform.position = Vector3.MoveTowards(transform.position, nodePosition.position, 1f);
            if (Vector3.Distance(transform.position,nodePosition.position) < 1)
            {
                walk = false;
                animator.SetBool("Walk", walk);
                active = true;
            }
        }
        else if (active)
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
                RotatingPiece.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
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
        UsableRange = StatRange;
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
        if (currentHP < 0)
        {
            Die();
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
        if (hunger > 0)
        {
            GameObject Projectile = Instantiate(Projectiles[UnityEngine.Random.Range(0, Projectiles.Count)], RotatingPiece.position, Quaternion.identity);
            Projectile.GetComponent<FlameProjectile>().napalmSentry = this;
            Rigidbody rb = Projectile.GetComponent<Rigidbody>();
            rb.AddForce((new Vector3(Target.position.x, Target.position.y, Target.position.z) - Projectile.transform.position) * 2, ForceMode.VelocityChange);
            hunger -= 1 * hungerModifier;
            if (!audioSource.isPlaying) audioSource.PlayOneShot(shoot);
        }
        else
        {
            audioSource.Stop();
        }
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
        nodePosition = nodeTransform;        
        walk = true;
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        active = false;
    }

    public void CashBack()
    {
        BuildingManager.Instance.bioMaterial += Convert.ToInt32(price * 0.25f);
    }
}
