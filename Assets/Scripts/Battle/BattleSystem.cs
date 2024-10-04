using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    List<Button> actionButtons;


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

        // Stop simulate physics
        playerCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        enemyCopy.gameObject.GetComponent<Rigidbody2D>().simulated = false;

        // Update UI
        BattleUIHandler.instance.GetPlayerEnemy(playerCopy, enemyCopy);
        BattleUIHandler.instance.UpdateStatus();   

        // Get action buttons
        actionButtons = BattleUIHandler.instance.actionButtons; 
        battleState = BattleState.PLAYERTURN;   // Set battle state
        PlayerTurn();
    }

    void PlayerTurn()
    {
        InitializeActionButtons();
        BattleUIHandler.instance.UpdateDialog("Choose your action");
    }

    void InitializeActionButtons()
    {
        BattleUIHandler.instance.EnableActions();
        // Set text on button
        actionButtons[0].text = "Attack";
        // Set the click event
        actionButtons[0].clicked += OnAttackClicked;

        actionButtons[1].text = "Skill";
        actionButtons[1].clicked += OnSkillClicked;

        actionButtons[2].text = "Guard";
        actionButtons[2].clicked += OnGuardClicked;

        actionButtons[3].text = "Item";
        actionButtons[3].clicked += OnItemClicked;

        actionButtons[4].text = "Escape";
        actionButtons[4].clicked += OnEscapeClicked;

        for (int i = 0; i < 5; i++)
        {
            // Set the hover event (mouse enter)
            actionButtons[i].RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
        }

        // Set the hover event (mouse leave)
        BattleUIHandler.instance.actions.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);

        // Hide not used buttons
        actionButtons[5].style.visibility = Visibility.Hidden;
        actionButtons[6].style.visibility = Visibility.Hidden;
        actionButtons[7].style.visibility = Visibility.Hidden;
    }

    private void DisplayButtonDescription(MouseEnterEvent evt)
    {
        Button button = (Button)evt.target;  // Get the button from the event
        switch(button.text)
        {
            case("Attack"):
                BattleUIHandler.instance.UpdateDialog($"Deals {CalculateDamage()} physical damage to the enemy.\nCost: {"0"} MP");
                break;
            case ("Skill"):
                BattleUIHandler.instance.UpdateDialog("Select skill from learned skills");
                break;
            case ("Guard"):
                BattleUIHandler.instance.UpdateDialog("Guard the next attack, half the damage");
                break;
            case ("Item"):
                BattleUIHandler.instance.UpdateDialog("Select item from inventory");
                break;
            case ("Escape"):
                BattleUIHandler.instance.UpdateDialog("Escape current battle");
                break;
        }
    }

    private void HideButtonDescription(MouseLeaveEvent evt)
    {
        BattleUIHandler.instance.UpdateDialog("Choose your action");
    }


    private void OnEscapeClicked()
    {
        throw new NotImplementedException();
    }

    private void OnItemClicked()
    {
        throw new NotImplementedException();
    }

    private void OnGuardClicked()
    {
        StartCoroutine(PlayerGuard());
    }

    IEnumerator PlayerGuard()
    {
        BattleUIHandler.instance.DisableActions();
        BattleUIHandler.instance.UpdateDialog("Guarding");
        playerCopy.guard = true;

        yield return new WaitForSeconds(1f);

        battleState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    private void OnSkillClicked()
    {
        throw new NotImplementedException();
    }

    void OnAttackClicked()
    {
        BattleUIHandler.instance.DisableActions();
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {

        int damage = playerCopy.damage - enemyCopy.defense;
        enemyCopy.TakeDamage(damage);
        BattleUIHandler.instance.UpdateStatus();
        BattleUIHandler.instance.UpdateDialog("Attack is successfull");

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
        BattleUIHandler.instance.DisableActions();
        BattleUIHandler.instance.UpdateDialog(enemyCopy.battleObjectName + " attacks!");


        yield return new WaitForSeconds(1f);

        int damage = CalculateDamage();
        playerCopy.TakeDamage(damage);
        BattleUIHandler.instance.UpdateStatus();
        BattleUIHandler.instance.UpdateDialog(enemyCopy.battleObjectName + " deals " + damage + " damage to you");

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

    private int CalculateDamage()
    {
        int damage = enemyCopy.damage - playerCopy.defense;
        if (playerCopy.guard)
        {
            damage /= 2;
            playerCopy.guard = false;
        }
        return damage;
    }

    IEnumerator EndBattle()
    {
        if (battleState == BattleState.WIN)
        {
            BattleUIHandler.instance.UpdateDialog("You win");
            yield return new WaitForSeconds(2f);
            SceneManagerScript.instance.LoadMapScene(playerCopy);    // Send data back
        } 
        else if (battleState == BattleState.LOSE)
        {
            BattleUIHandler.instance.UpdateDialog("You lose");
            // Load Lose Scene
        }
    }





}
