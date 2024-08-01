using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimAssist : MonoBehaviour
{
    Boss boss;
    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }
    public void SetAttackBool()
    {
        if(boss.AttackIng)boss.AttackIng = false;
        else boss.AttackIng = true;
    }
    public void Spin()
    {
        boss.AttackOne();
    }
    public void Slam()
    {
        boss.AttackTwo();
    }
    public void Shoot() 
    {
        boss.AttackThree();
    }
}
