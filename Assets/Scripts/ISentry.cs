using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISentry 
{
    abstract public List<GameObject> GetMeshes();
    abstract public GameObject GetGO();
    abstract public GameObject GetRangeVisual();
    abstract void TakeDMG();
    abstract void Activate();
}
