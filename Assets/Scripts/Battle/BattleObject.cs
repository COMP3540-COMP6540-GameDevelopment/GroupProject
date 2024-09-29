using System.Collections;
using UnityEngine;


public class BattleObject : MonoBehaviour
{
    public string battleObjectName;
    public int currentHP;
    public int maxHP;
    public int level;
    public int damage;
    public int defense;
    public int exp;
    public int gold;
    public bool Dead = false;

    public bool IsDead() 
    { 
        return Dead; 
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        if (damage >= currentHP)
        {
            currentHP = 0;
            Dead = true;
        }
        else
        {
            currentHP -= damage;
        }
    }
}
