using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE}

public class BattleSystem : MonoBehaviour
{
    public Transform playerPosition;
    public Transform enemyPosition;
    public GameObject player;
    public GameObject enemy;
    public BattleScript playerCopy;
    public BattleScript enemyCopy;
    public BattleState battleState;
    private BattleUISystem battleUISystem;

    void Start()
    {
        battleUISystem = GameObject.Find("Battle UI System").GetComponent<BattleUISystem>();

        battleState = BattleState.START;    // Set battle state
        
        // Receive player and enemy data from scene manager
        player = SceneManagerScript.instance.playerPrefab;
        enemy = SceneManagerScript.instance.enemy;

        // create a copy to commence battle
        playerCopy = Instantiate(player, playerPosition).GetComponent<BattleScript>();
        enemyCopy = Instantiate(enemy, enemyPosition).GetComponent<BattleScript>();

        playerCopy.transform.localPosition = Vector3.zero;
        enemyCopy.transform.localPosition = Vector3.zero;

        playerCopy.gameObject.SetActive(true);
        enemyCopy.gameObject.SetActive(true);

        playerCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        enemyCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;


        battleState = BattleState.PLAYERTURN;   // Set battle state
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

        int damage = playerCopy.damage - enemyCopy.defense;
        enemyCopy.TakeDamage(damage);
        battleUISystem.UpdateStatus();
        battleUISystem.UpdateDialog("Attack is successfull");

        yield return new WaitForSeconds(1f);
        
        if (enemyCopy.IsDead())
        {
            battleState = BattleState.WIN;

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
        battleUISystem.ActionUI.SetActive(false);
        battleUISystem.UpdateDialog(enemyCopy.battleObjectName + " attacks!");
        

        yield return new WaitForSeconds(1f);

        int damage = enemyCopy.damage - playerCopy.defense;
        playerCopy.TakeDamage(damage);
        battleUISystem.UpdateStatus();
        battleUISystem.UpdateDialog(enemyCopy.battleObjectName + " deals " + damage + " damage to you");

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
            battleUISystem.UpdateDialog("You win");
            yield return new WaitForSeconds(2f);
            SceneManagerScript.instance.LoadMapScene(playerCopy);    // Send data back
        } 
        else if (battleState == BattleState.LOSE)
        {
            battleUISystem.UpdateDialog("You lose");
            // Load Lose Scene
        }
    }

}
