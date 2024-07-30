using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISentry 
{
    abstract public List<GameObject> GetMeshes();
    abstract public GameObject GetGO();
    abstract public GameObject GetRangeVisual();
    abstract void TakeDMG(int DMG);
    abstract void Activate();
    abstract void Feed(float foodAmount);
    abstract public void SpendCredit();
    abstract public bool EnoughMaterial();
    abstract public void Die();
    abstract public void CashBack();
    abstract public void WalkTo(Transform nodeTransform);
}
