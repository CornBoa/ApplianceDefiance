using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevAnimAssist : MonoBehaviour
{
    Elevator elevator;
    ParticleSystem steamSystem;
    void Start()
    {
        elevator = GetComponentInParent<Elevator>();
        steamSystem = GetComponentInChildren<ParticleSystem>();
    }
    public void OnRisen()
    {
        elevator.OnRisen();
    }
    public void PlaySteam()
    {
        steamSystem.Play();
    }
    public void UnOccupy()
    {
        elevator.occupied = false;
    }
}
