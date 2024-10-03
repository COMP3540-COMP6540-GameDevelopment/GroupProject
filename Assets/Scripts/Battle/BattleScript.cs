using System;
using System.Collections;
using UnityEngine;


public class BattleScript : MonoBehaviour
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

    internal void ReceiveLoot(BattleScript enemyCopy)
    {
        exp += enemyCopy.exp;
        gold += enemyCopy.gold;
    }

    internal void UpdateResults(BattleScript playerCopy)
    {
      currentHP = playerCopy.currentHP;
      maxHP = playerCopy.maxHP;
      level = playerCopy.level;
      damage = playerCopy.damage;
      defense = playerCopy.defense;
      exp = playerCopy.exp;
      gold = playerCopy.gold;
}
}
