using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    abstract void TakeDMG(int DMG);
    abstract void DealSentryDMG(int DMG);
    abstract void DealHouseDMG(int DMG);
    abstract void SetLayout(MapLayout map);
}
