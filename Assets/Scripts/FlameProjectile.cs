using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    public NapalmSentry napalmSentry;
    Rigidbody body;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,napalmSentry.transform.position) > napalmSentry.UsableRange) 
        {
            Destroy(gameObject);
        }
    }
}
