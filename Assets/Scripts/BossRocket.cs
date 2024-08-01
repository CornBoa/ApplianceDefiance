using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRocket : MonoBehaviour
{
    Boss boss;
    public float speed = 15f;
    bool choseTarget = false;
    public GameObject Target;
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
            transform.LookAt(Vector3.up);
        }
        else if (!choseTarget)
        {
            int randNum = Random.Range(0, FindObjectsOfType<NodeTest>().Length);
            Target = FindObjectsOfType<NodeTest>()[randNum].gameObject;
            if(Target != null)choseTarget= true;
        }
        if(Target != null) 
        {
            transform.LookAt(Target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position,Target.transform.position,speed * Time.deltaTime);
        }
        if (Target != null && Vector3.Distance(Target.transform.position, transform.position) < 1 && choseTarget)
        {
            Explode();
        }
    }
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<ISentry>() != null) collider.GetComponent<ISentry>().TakeDMG(DMG);
            }
            Debug.Log("DMG dealt");
        }
        DestroyImmediate(gameObject);
    }
}
