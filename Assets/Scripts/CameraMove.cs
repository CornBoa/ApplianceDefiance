using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Events;

public class CameraMove : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    public bool move = false;
    bool didIt;
    public UnityEvent cameraRollEnd;
    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }
    public void StartMobving()
    {
        move = true;
    }
    void Update()
    {
        if (move) 
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney * fractionOfJourney);
            transform.rotation = Quaternion.Lerp(startMarker.rotation, endMarker.rotation, fractionOfJourney);
        }      
        if (Vector3.Distance(transform.position, endMarker.position) == 0 && !didIt)
        {
            didIt = true;
            cameraRollEnd.Invoke();
        }
    }
}