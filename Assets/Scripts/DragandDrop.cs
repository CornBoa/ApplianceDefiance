using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragandDrop : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{
    static public DragandDrop instance;
    public bool beingDragged = false;
    public float satisfaction;
    Vector3 startPos;
    public NodeTest TargetNode;
    private void Awake()
    {
        startPos = transform.position;
        instance = this;            
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        beingDragged = false;
        transform.position = startPos;
        if (TargetNode != null)
        {
            TargetNode.sentryInstalled.Feed(satisfaction);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beingDragged = true;
    }
}
