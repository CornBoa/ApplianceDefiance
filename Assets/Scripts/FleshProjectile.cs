using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshProjectile : MonoBehaviour
{
    public Transform Target;
    public float speed;
    public enum KindOfProjectile
    {
        Teeth,
        Rot,
        Napalm
    }
    void Start()
    {
        
    }

    private void Update()
    {
        transform.LookAt(Target.position);
        Vector3 movement = new Vector3(0, 0, transform.position.z) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
