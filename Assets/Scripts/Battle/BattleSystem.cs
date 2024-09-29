using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerPosition;
    public Transform enemyPosition;
    public BattleObject player;
    public BattleObject enemy;
    public BattleState battleState;
    private BattleUISystem battleUISystem;

    void Start()
    {
        battleUISystem = GameObject.Find("Battle UI System").GetComponent<BattleUISystem>();

        battleState = BattleState.START;
        player = Instantiate(playerPrefab, playerPosition).GetComponent<BattleObject>();
        enemy = Instantiate(enemyPrefab, enemyPosition).GetComponent<BattleObject>();
        battleState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        
        battleUISystem.ActionUI.SetActive(true);
        battleUISystem.UpdateDialog("Choose your action");
    }

    public void OnAttackClicked()
    {
        if (battleState != BattleState.PLAYERTURN)
        {
            return;
        }
        battleUISystem.ActionUI.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {

        int damage = player.damage - enemy.defense;
        enemy.TakeDamage(damage);
        battleUISystem.UpdateStatus();
        battleUISystem.UpdateDialog("Attack is successfull");

        yield return new WaitForSeconds(1f);
        
        if (enemy.IsDead())
        {
            battleState = BattleState.WIN;
            EndBattle();  
        }
        else
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        
    }

    IEnumerator EnemyTurn()
    {
        battleUISystem.ActionUI.SetActive(false);
        battleUISystem.UpdateDialog(enemy.battleObjectName + " attacks!");
        

        yield return new WaitForSeconds(1f);

        int damage = enemy.damage - player.defense;
        player.TakeDamage(damage);
        battleUISystem.UpdateStatus();
        battleUISystem.UpdateDialog(enemy.battleObjectName + " deals " + damage + " damage to you");

        yield return new WaitForSeconds(1f);

        if (player.IsDead())
        {
            battleState = BattleState.LOSE;
            EndBattle();
        }
        else
        {
            battleState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void EndBattle()
    {
        if (battleState == BattleState.WIN)
        {
            battleUISystem.UpdateDialog("You win");
        } 
        else if (battleState == BattleState.LOSE)
        {
            battleUISystem.UpdateDialog("You lose");
        }
    }

}
