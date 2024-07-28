using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform SpawnPos;
    Animator animator;
    public ISentry handledSentry;
    NodeTest nodeCall;
    public bool occupied;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void Spawn(NodeTest node)
    {
        occupied = true;
        nodeCall = node;
        handledSentry.GetGO().transform.position = SpawnPos.position;
        handledSentry.GetGO().transform.parent = SpawnPos;
        animator.SetBool("Risen", true);
    }
    public void OnRisen()
    {
        handledSentry.GetGO().transform.parent = null;
        handledSentry.WalkTo(nodeCall.transform);
        StartCoroutine(WaitToClose());
    }
    IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("Risen", false);
    }
}
