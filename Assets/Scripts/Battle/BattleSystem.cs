using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    List<Button> skillButtons;


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
        skillButtons = BattleUIHandler.instance.skillButtons;
        InitializeActionButtons();
        InitiallizeSkillButtons();
        battleState = BattleState.PLAYERTURN;   // Set battle state
        PlayerTurn();
    }

    private void InitiallizeSkillButtons()
    {
        throw new NotImplementedException();
    }

    void PlayerTurn()
    {
        BattleUIHandler.instance.EnableActions();
        BattleUIHandler.instance.UpdateDialog("Choose your action");
    }

    void InitializeActionButtons()
    {
        BattleUIHandler.instance.EnableActions();
        // Set text on button
        actionButtons[0].text = "Attack";
        actionButtons[0].userData = playerCopy.attackSkill;
        actionButtons[1].text = "Skill";
        actionButtons[2].text = "Guard";
        actionButtons[3].text = "Item";
        actionButtons[4].text = "Escape";


        for (int i = 0; i < 5; i++)
        {
            // Set the click event
            actionButtons[i].RegisterCallback<ClickEvent>(OnButtonClicked);
            // Set the hover event (mouse enter)
            actionButtons[i].RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
        }
        // Set the hover event (mouse leave)
        BattleUIHandler.instance.actions.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);

        // Display used buttons
        actionButtons[0].style.visibility = Visibility.Visible;
        actionButtons[1].style.visibility = Visibility.Visible;
        actionButtons[2].style.visibility = Visibility.Visible;
        actionButtons[3].style. visibility = Visibility.Visible;
        actionButtons[4].style.visibility = Visibility.Visible;

        // Hide not used buttons
        actionButtons[5].style.visibility = Visibility.Hidden;
        actionButtons[6].style.visibility = Visibility.Hidden;
        actionButtons[7].style.visibility = Visibility.Hidden;
    }

    void UnregisterEvents()
    {
        foreach (var action in actionButtons)
        {
            action.UnregisterCallback<ClickEvent>(OnButtonClicked);
            action.UnregisterCallback<MouseEnterEvent>(DisplayButtonDescription);
        }
    }

    void OnButtonClicked(ClickEvent evt)
    {
        Button button = (Button)evt.target;  // Get the button from the event
        if (button.text == "Attack")
        {
            OnAttackClicked(evt);
        } 
        else if (button.text == "Skill")
        {
            OnSkillClicked(evt);
        }
        else if (button.text == "Guard")
        {
            OnGuardClicked(evt);
        }
        else if (button.text == "Item")
        {
            OnItemClicked(evt);
        }
        else if (button.text == "Escape")
        {
            OnEscapeClicked(evt);
        }
    }

    private void DisplayButtonDescription(MouseEnterEvent evt)
    {
        Button button = (Button)evt.target;  // Get the button from the event
        Skill skill = button.userData as Skill;
        String dialogText = "";
        if (skill != null)
        {
            int damage = CalculateDamage(playerCopy, enemyCopy, skill);
            dialogText += skill.description + ".\n";
            dialogText += $"Deals <color=red>{damage}</color> {skill.skillType} damage to the enemy" + ".\n";
            dialogText += $"Potency: {skill.potency}\tCost: <color=blue>{skill.costMP}</color> MP";
        }
        switch (button.text)
        {
            case ("Skill"):
                dialogText += "Select skill from learned skills";
                break;
            case ("Guard"):
                dialogText += "Guard the next attack, half the damage";
                break;
            case ("Item"):
                dialogText += "Select item from inventory";
                break;
            case ("Escape"):
                dialogText += "Escape current battle";
                break;
        }

        BattleUIHandler.instance.UpdateDialog(dialogText);
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
        // Assign each skill to action buttons
        foreach (var action in actionButtons)
        {
            action.style.visibility = Visibility.Hidden;
        }
        int i = 0;
        foreach (Skill skill in playerSkills)
        {
            if (skill == null)
            {
                // Break if no skills
                break;
            }
            String skillName = skill.name;

            
            actionButtons[i].style.visibility = Visibility.Visible;
            actionButtons[i].text = skillName;
            actionButtons[i].userData = skill;
            actionButtons[i].UnregisterCallback<ClickEvent>(OnSkillClicked);
            actionButtons[i].RegisterCallback<ClickEvent>(OnAttackClicked);
            i++;
        }
    }

    void OnAttackClicked(ClickEvent evt)
    {
        // Get the button that triggered this event
        Button button = evt.target as Button;
        // Get userdata as Skill
        Skill skill = button.userData as Skill;

        BattleUIHandler.instance.DisableActions();

        StartCoroutine(PlayerAttack(skill));
    }

    IEnumerator PlayerAttack(Skill skill)
    {
        int damage = 0;
        SkillType skillType = skill.skillType;
        if (skillType == SkillType.DEFAULT)
        {
            damage = CalculateDamage(playerCopy, enemyCopy);
            
        } else
        {
            damage = CalculateDamage(playerCopy, enemyCopy, skill);
        }

        enemyCopy.TakeDamage(damage);
        BattleUIHandler.instance.UpdateStatus();
        BattleUIHandler.instance.UpdateDialog($"Hero deals <color=red>{damage}</color> {skillType} damage to {enemyCopy.battleObjectName}");

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
    private int CalculateDamage(BattleScript from, BattleScript to, Skill skill)
    {
        if (skill.skillType == SkillType.DEFAULT)
        {
            return CalculateDamage(from, to);
        } else
        {
            
            int damage = Mathf.RoundToInt(from.damage * (float) skill.potency / 100f - to.defense);
            if (to.guard)
            {
                damage /= 2;
                to.guard = false;
            }
            return damage;
        }
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
