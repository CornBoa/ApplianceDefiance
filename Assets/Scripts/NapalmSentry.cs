using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmSentry : MonoBehaviour , ISentry
{
    private float Range;
    public float PublicRange;
    public Transform Target, RotatingPiece;
    SphereCollider SphereCollider;
    public GameObject RangeVis;
    public List<Transform> EnemyQueue = new List<Transform>();
    public List<GameObject> meshesObjects;
    float HP = 10;
    public float hunger = 10;
    public float hungerModifier;
    public ParticleSystem flames;
    float nextFireTime;
    public float fireRate;
    public bool active;
    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public void Feed(float foodAmount)
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGO()
    {
        throw new System.NotImplementedException();
    }

    public List<GameObject> GetMeshes()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetRangeVisual()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDMG(int DMG)
    {
        throw new System.NotImplementedException();
    }
    void Shoot()
    {
        flames.Emit(3);
        hunger -= 1 * hungerModifier;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
