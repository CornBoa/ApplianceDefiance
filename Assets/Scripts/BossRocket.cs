using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocket : MonoBehaviour
{
    Boss boss;
    public float speed = 5f;
    bool choseTarget;
    GameObject Target;
    public int DMG;
    void Start()
    {
        boss = FindObjectOfType<Boss>();
    }
    void Update()
    {
        if (Vector3.Distance(boss.transform.position, transform.position) < 40)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (!choseTarget)
        {
            int randNum = Random.Range(0, FindObjectsOfType<NodeTest>().Length);
            Target = FindObjectsOfType<NodeTest>()[randNum].gameObject;
            choseTarget= true;
        }
        if(Target != null) 
        {
            transform.position = Vector3.MoveTowards(transform.position,Target.transform.position,speed * Time.deltaTime);
        }
        if (Vector3.Distance(Target.transform.position, transform.position) < 40)
        {
            Explode();
        }
    }
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<ISentry>() != null) collider.GetComponent<ISentry>().TakeDMG(DMG);
            }
            Debug.Log("DMG dealt");
        }
    }
}
