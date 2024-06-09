using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTest : MonoBehaviour
{
    public bool occupied,buffer,road;
    Renderer rend;
    public ISentry sentryInstalled;
    GameObject sentryHolo;
    public DragandDrop Meat;
    AudioSource PlapSource;
    public AudioClip PlapClip;
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(road) rend.material.color = Color.black;
        Meat = DragandDrop.instance;
        PlapSource = GetComponentInParent<AudioSource>();
    }

    private void OnMouseEnter()
    {
        if (!buffer && !road)
        {
            if (!occupied && BuildingManager.Instance.currentSentry != null)
            {
                rend.material.color = Color.green;
                if (sentryHolo == null) sentryHolo = Instantiate(BuildingManager.Instance.currentSentry.GetGO(), transform.position, transform.rotation);
            }
            else if (BuildingManager.Instance.currentSentry != null) rend.material.color = Color.red;
            if (Meat.beingDragged && sentryInstalled != null)
            {
                Meat.TargetNode = this;
                rend.material.color = Color.blue;
            }
            if(BuildingManager.Instance.buildMode) BuildingManager.Instance.currentNode = this;
        }            
    }
    private void OnMouseExit()
    {
        if (!buffer && !road)
        {
            Destroy(sentryHolo);
            rend.material.color = Color.gray;
            if(Meat.TargetNode == this) Meat.TargetNode = null;
            if (BuildingManager.Instance.currentNode == this) BuildingManager.Instance.currentNode = null ;
        }   
    }
    private void OnMouseDown()
    {
        if (BuildingManager.Instance.currentSentry != null && !occupied && !buffer && !road && BuildingManager.Instance.IsEnoughMoney())
        {
            sentryInstalled = Instantiate(BuildingManager.Instance.currentSentry.GetGO(), transform.position, transform.rotation).GetComponent<ISentry>();
            occupied = true;
            sentryInstalled.Activate();
            foreach (GameObject mesh in sentryInstalled.GetMeshes())
            {
                mesh.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
            GameObject RangeVis  = sentryInstalled.GetRangeVisual();
            RangeVis.SetActive(false);
            Destroy(sentryHolo);
            rend.material.color = Color.red;
            PlapSource.pitch = Random.Range(0.95f, 1.1f);
            PlapSource.PlayOneShot(PlapClip);
            BuildingManager.Instance.MoneySpend();
        }
        
    }
}
