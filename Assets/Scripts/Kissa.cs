using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kissa : MonoBehaviour
{
    AudioSource m_AudioSource;
    public List <AudioClip> Meows;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
       m_AudioSource.PlayOneShot(Meows[Random.Range(0, Meows.Count - 1)]);
    }
}
