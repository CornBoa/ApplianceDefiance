using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    abstract void TakeDMG(int DMG);
    abstract void DealSentryDMG();
    abstract void DealHouseDMG(int DMG);
    abstract void SetLayout(MapLayout map);
    abstract void GetElectrocuted(List<IEnemy> shocked, int DMG, ElectroSentry sentry);
    abstract GameObject GetGO();
}
