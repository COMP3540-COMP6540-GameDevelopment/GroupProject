using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE}

public class BattleSystem : MonoBehaviour
{
    public Transform playerPosition;
    public Transform enemyPosition;
    public BattleScript playerCopy;
    public BattleScript enemyCopy;
    public BattleState battleState;


    void Start()
    {
        battleState = BattleState.START;    // Set battle state

        // Receive player and enemy data from scene manager, create a copy to commence battle
        playerCopy = Instantiate(SceneManagerScript.instance.player, playerPosition).GetComponent<BattleScript>();
        enemyCopy = Instantiate(SceneManagerScript.instance.enemy, enemyPosition).GetComponent<BattleScript>();

        playerCopy.transform.localPosition = Vector3.zero;
        enemyCopy.transform.localPosition = Vector3.zero;

        playerCopy.gameObject.SetActive(true);
        enemyCopy.gameObject.SetActive(true);

        playerCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        enemyCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;

        BattleUIHandler.instance.GetPlayerEnemy(playerCopy, enemyCopy);
        BattleUIHandler.instance.UpdateStatus();   // Update UI

        battleState = BattleState.PLAYERTURN;   // Set battle state
        PlayerTurn();
    }

    void PlayerTurn()
    {
        BattleUIHandler.instance.EnableActions();
        BattleUIHandler.instance.UpdateDialog("Choose your action");
        //battleUISystem.UpdateDialog("Choose your action");
    }

    public void OnAttackClicked()
    {
        if (battleState != BattleState.PLAYERTURN)
        {
            return;
        }
        //battleUISystem.ActionUI.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {

        int damage = playerCopy.damage - enemyCopy.defense;
        enemyCopy.TakeDamage(damage);
        //battleUISystem.UpdateStatus();
        //battleUISystem.UpdateDialog("Attack is successfull");

        yield return new WaitForSeconds(1f);
        
        if (enemyCopy.IsDead())
        {
            battleState = BattleState.WIN;
            playerCopy.ReceiveLoot(enemyCopy);
            StartCoroutine(EndBattle());
        }
        else
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        
    }

    IEnumerator EnemyTurn()
    {
        //battleUISystem.ActionUI.SetActive(false);
        //battleUISystem.UpdateDialog(enemyCopy.battleObjectName + " attacks!");
        

        yield return new WaitForSeconds(1f);

        int damage = enemyCopy.damage - playerCopy.defense;
        playerCopy.TakeDamage(damage);
        //battleUISystem.UpdateStatus();
        //battleUISystem.UpdateDialog(enemyCopy.battleObjectName + " deals " + damage + " damage to you");

        yield return new WaitForSeconds(1f);

        if (playerCopy.IsDead())
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

    IEnumerator EndBattle()
    {
        if (battleState == BattleState.WIN)
        {
            //battleUISystem.UpdateDialog("You win");
            yield return new WaitForSeconds(2f);
            SceneManagerScript.instance.LoadMapScene(playerCopy);    // Send data back
        } 
        else if (battleState == BattleState.LOSE)
        {
            //battleUISystem.UpdateDialog("You lose");
            // Load Lose Scene
        }
    }

}
