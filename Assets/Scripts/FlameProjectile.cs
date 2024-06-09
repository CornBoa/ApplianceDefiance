using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    public NapalmSentry napalmSentry;
    Rigidbody body;
    float t = 0;
    public float ShrinkTime;
    bool goesOut;
    public int DMG;
    MeshRenderer rendererMAt;
    void Start()
    {
        rendererMAt = GetComponentInChildren<MeshRenderer>();
        body = GetComponent<Rigidbody>();
        rendererMAt.material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,napalmSentry.transform.position) > napalmSentry.UsableRange && !goesOut) 
        {
            goesOut = true;        
        }
        if(goesOut) 
        {
            body.AddForce(Vector3.up * 0.2f, ForceMode.Impulse);
            transform.localScale /= (Time.time * 0.2f);
            t += Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, t / ShrinkTime), Mathf.Lerp(1f, 0f, t / ShrinkTime), Mathf.Lerp(1f, 0f, t / ShrinkTime));
            ColorChangerr();
            if (transform.localScale == Vector3.zero)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            goesOut = true;
            other.GetComponent<IEnemy>().TakeDMG(DMG);
        }
        else if (other.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }
    void ColorChangerr()
    {
            rendererMAt.material.color = Color.Lerp(rendererMAt.material.color, Color.black, t / ShrinkTime);
    }
}
