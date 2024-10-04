using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleScript : MonoBehaviour
{
    public string battleObjectName;
    public int currentHP;
    public int maxHP; 
    public int currentMP;
    public int maxMP;
    public int level;
    public int damage;
    public int defense;
    public int exp;
    public int gold;
    public bool dead = false;
    public bool guard = false;
    public List<Skill> skills;

    public bool IsDead() 
    { 
        return dead; 
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;
        if (damage >= currentHP)
        {
            currentHP = 0;
            dead = true;
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
        currentMP = playerCopy.currentMP;
        maxMP = playerCopy.maxMP;
        level = playerCopy.level;
        damage = playerCopy.damage;
        defense = playerCopy.defense;
        exp = playerCopy.exp;
        gold = playerCopy.gold;
    }

}
