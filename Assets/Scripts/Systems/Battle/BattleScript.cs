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
    public Skill attackSkill;
    public List<Skill> skills;
    public int boughtCount = 0;

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
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public void RecoverHP(int value)
    {
        if (value < 0) return;
        if (value + currentHP > maxHP)
        {
            currentHP = maxHP;
        } else
        {
            currentHP += value;
        }
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public void RecoverMP(int value)
    {
        if (value < 0) return;
        if (value + currentMP > maxMP)
        {
            currentMP = maxMP;
        }
        else
        {
            currentMP += value;
        }
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public void FullyRecover()
    {
        RecoverHP(9999);
        RecoverMP(9999);
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    internal void ReceiveLoot(BattleScript enemyCopy)
    {
        exp += enemyCopy.exp;
        gold += enemyCopy.gold;
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    internal void ReduceMana(int costMP)
    {
        currentMP -= costMP;
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
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
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public int CalculateNeedEXPToLevelUp()
    {
        return 10 * level * (level - 1) + 20;
    }

    public int CalculateNeedGOLDToBuy()
    {
        if (boughtCount == 0)
        {
            return 20;
        }
        else
        {
            return 10 * boughtCount * (boughtCount - 1) + 20;
        }
    }

    public void LevelUp()
    {
        exp -= CalculateNeedEXPToLevelUp();
        level += 1;
        maxHP += 100;
        currentHP += 100;
        maxMP += 100;
        currentMP += 100;
        damage += 2;
        defense += 2;
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public void IncreaseDMG()
    {
        gold -= CalculateNeedGOLDToBuy();
        boughtCount++;
        damage += 5;
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

    public void IncreaseDEF()
    {
        gold -= CalculateNeedGOLDToBuy();
        boughtCount++;
        defense += 5;
        gameObject.GetComponent<DisplayHUD>().UpdateStatus();
    }

}
