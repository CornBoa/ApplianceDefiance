using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTest : MonoBehaviour
{
    public bool occupied,buffer,road;
    Renderer rend;
    ISentry sentryInstalled;
    GameObject sentryHolo;
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(road) rend.material.color = Color.black;
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
        }            
    }
    private void OnMouseExit()
    {
        if (!buffer && !road)
        {
            Destroy(sentryHolo);
            rend.material.color = Color.gray;
        }   
    }
    private void OnMouseDown()
    {
        if (BuildingManager.Instance.currentSentry != null && !occupied && !buffer && !road)
        {
            sentryInstalled = Instantiate(BuildingManager.Instance.currentSentry.GetGO(), transform.position, transform.rotation).GetComponent<ISentry>();
            occupied = true;
            foreach (GameObject mesh in sentryInstalled.GetMeshes())
            {
                mesh.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
            GameObject RangeVis  = sentryInstalled.GetRangeVisual();
            RangeVis.SetActive(false);
        }
        
    }
}
