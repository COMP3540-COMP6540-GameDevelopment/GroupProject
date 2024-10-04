using System;
using System.Collections;
using System.Collections.Generic;
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



    void PlayerTurn()
    {
        BattleUIHandler.instance.EnableActions();
        BattleUIHandler.instance.UpdateDialog("Choose your action");
    }
    void InitiallizeSkillButtons()
    {
        BattleUIHandler.instance.DisableSkills();
        List<Skill> playerSkills = playerCopy.skills;
        // Assign each skill to action buttons
        int i = 0;
        foreach (Skill skill in playerSkills)
        {
            if (skill == null)
            {
                // Break if no skills
                break;
            }
            skillButtons[i].text = skill.name;
            skillButtons[i].userData = skill;
            skillButtons[i].RegisterCallback<ClickEvent>(OnAttackClicked);
            skillButtons[i].RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
            i++;
        }
        BattleUIHandler.instance.skills.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);
    }

    void InitializeActionButtons()
    {
        // Set text on button
        actionButtons[0].text = "Attack";
        actionButtons[0].userData = playerCopy.attackSkill;
        actionButtons[1].text = "Skill";
        actionButtons[2].text = "Guard";
        actionButtons[3].text = "Item";
        actionButtons[4].text = "Escape";

        foreach (var action in actionButtons)
        {
            // Set the click event
            action.RegisterCallback<ClickEvent>(OnButtonClicked);
            // Set the hover event (mouse enter)
            action.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
            // Display used buttons
            action.style.visibility = Visibility.Visible;
        }
        // Set the hover event (mouse leave)
        BattleUIHandler.instance.actions.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);
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

    void DisplayButtonDescription(MouseEnterEvent evt)
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

    void HideButtonDescription(MouseLeaveEvent evt)
    {
        if (BattleUIHandler.instance.actions.style.visibility == Visibility.Visible)
        {
            BattleUIHandler.instance.UpdateDialog("Choose your action");
        } 
        else if (BattleUIHandler.instance.skills.style.visibility == Visibility.Visible)
        {
            BattleUIHandler.instance.UpdateDialog("Right click to go back to actions");
        }
    }


    void OnEscapeClicked(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    void OnItemClicked(ClickEvent evt)
    {
        throw new NotImplementedException();
    }

    void OnGuardClicked(ClickEvent evt)
    {
        StartCoroutine(PlayerGuard());
    }

    IEnumerator PlayerGuard()
    {
        BattleUIHandler.instance.DisableActions();
        BattleUIHandler.instance.DisableSkills();
        BattleUIHandler.instance.UpdateDialog("Guarding");
        playerCopy.guard = true;

        yield return new WaitForSeconds(1f);

        battleState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    void OnSkillClicked(ClickEvent evt)
    {
        BattleUIHandler.instance.EnableSkills();
        BattleUIHandler.instance.DisableActions();
        BattleUIHandler.instance.uiDocument.rootVisualElement.RegisterCallback<MouseDownEvent>(ReturnToActions);
    }
    void ReturnToActions(MouseDownEvent evt)
    {
        if (evt.button != 1)
        {
            return;
        }
        BattleUIHandler.instance.DisableSkills();
        BattleUIHandler.instance.EnableActions();
        BattleUIHandler.instance.uiDocument.rootVisualElement.UnregisterCallback<MouseDownEvent>(ReturnToActions);
    }

    void OnAttackClicked(ClickEvent evt)
    {
        BattleUIHandler.instance.DisableSkills();
        BattleUIHandler.instance.DisableActions();

        // Get the button that triggered this event
        Button button = evt.target as Button;
        // Get userdata as Skill
        Skill skill = button.userData as Skill;

        // Determine whether is is able to attack
        if (AbleToCast(skill))
        {
            // Commence attack with skill
            BattleUIHandler.instance.DisableSkills();
            BattleUIHandler.instance.DisableActions();
            StartCoroutine(PlayerAttack(skill));
        } 
        else
        {
            BattleUIHandler.instance.UpdateDialog("Not enough MP, can't cast skill");
        }
    }

    bool AbleToCast(Skill skill)
    {
        return playerCopy.currentMP >= skill.costMP;
        // TODO add cooldown
    }

    IEnumerator PlayerAttack(Skill skill)
    {
        int damage = 0;
        int costMP = 0;
        SkillType skillType = skill.skillType;
        if (skillType == SkillType.DEFAULT)
        {
            damage = CalculateDamage(playerCopy, enemyCopy);
            costMP = 0;
        } else
        {
            damage = CalculateDamage(playerCopy, enemyCopy, skill);
            costMP = skill.costMP;
        }
        playerCopy.ReduceMana(costMP);
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
        BattleUIHandler.instance.DisableSkills();
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
