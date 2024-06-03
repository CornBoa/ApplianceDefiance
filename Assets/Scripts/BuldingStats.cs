using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldingStats : MonoBehaviour
{
    public static BuldingStats instance;
    [SerializeField] public Dictionary<string, float> Stats = new Dictionary<string, float>();

}
