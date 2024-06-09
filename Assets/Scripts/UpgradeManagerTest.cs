using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManagerTest : MonoBehaviour
{
    //Teeth lvl 1 Upgrades
    void MoreMAXHP()
    {
        TeethSentryOne.maxHP++;
    }
    [ContextMenu("IncreaseRange")]
    void IncreaseRange()
    {
        TeethSentryOne.StatRange += 5;
    }
}
