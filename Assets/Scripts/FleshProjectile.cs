using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshProjectile : MonoBehaviour
{
    public Transform Target;
    public float speed;
    public int DMG;
    void Start()
    {
        
    }

    private void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
        }
        else if (Target != null)
        {
            transform.LookAt(Target.position);
            Vector3 movement = new Vector3(0, 0, transform.position.z) * speed * Time.deltaTime;
            transform.Translate(movement);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().TakeDMG(DMG);
        }
    }
}
