using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
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
        //actionButtons[0].clicked += OnAttackClicked;
        actionButtons[0].RegisterCallback<ClickEvent>(OnAttackClicked);
        actionButtons[0].userData = playerCopy.attackSkill;

        actionButtons[1].text = "Skill";
        actionButtons[1].RegisterCallback<ClickEvent>(OnSkillClicked);

        actionButtons[2].text = "Guard";
        actionButtons[2].RegisterCallback<ClickEvent>(OnGuardClicked);

        actionButtons[3].text = "Item";
        actionButtons[3].RegisterCallback<ClickEvent>(OnItemClicked);

        actionButtons[4].text = "Escape";
        actionButtons[4].RegisterCallback<ClickEvent>(OnEscapeClicked);

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
                int damage = CalculateDamage(playerCopy, enemyCopy);
                BattleUIHandler.instance.UpdateDialog($"Deals <color=red>{damage}</color> physical damage to the enemy.\nCost: <color=blue>{"0"}</color> MP");
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
        if (BattleUIHandler.instance.actions.style.visibility != Visibility.Hidden)
        {
            BattleUIHandler.instance.UpdateDialog("Choose your action");
        }
    }


    private void OnEscapeClicked(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    private void OnItemClicked(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    private void OnGuardClicked(ClickEvent evt)
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

    private void OnSkillClicked(ClickEvent evt)
    {
        List<Skill> playerSkills = playerCopy.skills;
        foreach (Skill skill in playerSkills)
        {
            if (skill == null)
            {
                break;
            }
            Debug.Log(skill);
        }
    }

    void OnAttackClicked(ClickEvent evt)
    {
        // Get the button that triggered this event
        Button button = evt.target as Button;
        // Get userdata as Skill
        Skill skill = button.userData as Skill;

        BattleUIHandler.instance.DisableActions();
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {

        int damage = CalculateDamage(playerCopy, enemyCopy);
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

        int damage = CalculateDamage(enemyCopy, playerCopy);
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

    private int CalculateDamage(BattleScript from, BattleScript to)
    {
        int damage = from.damage - to.defense;
        if (to.guard)
        {
            damage /= 2;
            to.guard = false;
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
