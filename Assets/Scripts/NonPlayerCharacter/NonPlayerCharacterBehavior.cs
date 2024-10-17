using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class NonPlayerCharacterBehavior : MonoBehaviour
{
    [SerializeField] GameObject hint;
    Rigidbody2D rb;

    // Variables related to conversation dialog
    UIDocument uiDocument;
    Label dialog;
    VisualElement dialogBackGround;
    [Tooltip("The text will be shown in the conversation from up to down")]
    [SerializeField] List<string> texts;
    [SerializeField] int indexOfShownText;
    Button closeButton;
    public GameObject whoIsTalkingTo;   // player
    BattleScript playerBattleScript;
    
    public bool isTradable;
    VisualElement leftActions;
    Button levelButton;
    Button damageButton;
    Button defenseButton;
    Button recoverButton;

    // Variables related to Animation
    Animator animator;
    [SerializeField] bool faceLeft;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        uiDocument = GetComponent<UIDocument>();
        animator = GetComponent<Animator>();

        whoIsTalkingTo = null;

        hint.SetActive(false);
        dialogBackGround = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Dialog_Background");
        dialogBackGround.RegisterCallback<ClickEvent>(OnDialogBackGroundClicked);
        
        dialog = dialogBackGround.Q<Label>("Dialog");
        indexOfShownText = 0;
        dialog.text = texts.ToArray()[indexOfShownText];

        closeButton = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions").Q<Button>("CloseButton");
        closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        leftActions = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions").Q<VisualElement>("Left");
        
        levelButton = leftActions.Q<Button>("Action1");
        damageButton = leftActions.Q<Button>("Action2");
        defenseButton = leftActions.Q<Button>("Action3");
        recoverButton = leftActions.Q<Button>("Action4");

        if (isTradable)
        {
            levelButton.RegisterCallback<ClickEvent>(OnLevelButtonClicked);
            damageButton.RegisterCallback<ClickEvent>(OnDamageButtonClicked);
            defenseButton.RegisterCallback<ClickEvent>(OnDefenseButtonClicked);
            recoverButton.RegisterCallback<ClickEvent>(OnRecoverButtonClicked);

            levelButton.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
            damageButton.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
            defenseButton.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
            recoverButton.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);

            leftActions.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);
        }


        Hide(uiDocument.rootVisualElement);
    }

    private void OnRecoverButtonClicked(ClickEvent evt)
    {
        int currentHP = playerBattleScript.currentHP;
        int currentMP = playerBattleScript.currentMP;
        int maxHP = playerBattleScript.maxHP;
        int maxMP = playerBattleScript.maxMP;

        if (currentHP == maxHP && currentMP == maxMP)
        {
            dialog.text = $"No need to recover.";
        } else
        {
            int goldNeed = 40;
            if (goldNeed <= playerBattleScript.gold)
            {
                playerBattleScript.gold -= goldNeed;
                playerBattleScript.FullyRecover();
                dialog.text = $"Fully recovered! You are now full of determination!";
            }
            else
            {
                dialog.text = $"Not enough gold.";
            }
        }

    }

    private void OnDefenseButtonClicked(ClickEvent evt)
    {
        int goldNeed = playerBattleScript.CalculateNeedGOLDToBuy();
        if (goldNeed <= playerBattleScript.gold)
        {
            playerBattleScript.IncreaseDEF();
            int defense = playerBattleScript.defense;
            dialog.text = $"Defense Increased! You have {defense} Defense now.";
        }
        else
        {
            dialog.text = $"Not enough gold.";
        }
    }

    private void OnDamageButtonClicked(ClickEvent evt)
    {
        int goldNeed = playerBattleScript.CalculateNeedGOLDToBuy();
        if (goldNeed <= playerBattleScript.gold)
        {
            playerBattleScript.IncreaseDMG();
            int damage = playerBattleScript.damage;
            dialog.text = $"Damage Increased! You have {damage} damage now.";
        }
        else
        {
            dialog.text = $"Not enough gold.";
        }
    }

    private void HideButtonDescription(MouseLeaveEvent evt)
    {
        dialog.text = texts.First();
    }

    private void DisplayButtonDescription(MouseEnterEvent evt)
    {
        int expNeed = playerBattleScript.CalculateNeedEXPToLevelUp();
        int exp = playerBattleScript.exp;
        int level = playerBattleScript.level;

        int goldNeed = playerBattleScript.CalculateNeedGOLDToBuy();
        int gold = playerBattleScript.gold;
        int damage = playerBattleScript.damage;
        int defense = playerBattleScript.defense;
        int boughtCount = playerBattleScript.boughtCount;

        Button button = (Button)evt.target;  // Get the button from the event
        if (button.text == "LEVEL")
        {
            dialog.text = $"Level up and increase Max HP, MP by <color=red>100</color>,\nDamage, Defense by <color=red>2</color>.\n";
            dialog.text += $"You are Level {level}. This will spend <color=red>{expNeed}</color> exp!\n";
            dialog.text += $"You have <color=red>{exp}</color> exp.\n";
        } 
        else if (button.text == "DMG")
        {
            dialog.text = $"Increase damage by 5.\n";
            dialog.text += $"This will spend <color=red>{goldNeed}</color> gold!\n";
            dialog.text += $"You have <color=red>{gold}</color> gold.\n";
        }
        else if (button.text == "DEF")
        {
            dialog.text = $"Increase defense by 5.\n";
            dialog.text += $"This will spend <color=red>{goldNeed}</color> gold!\n";
            dialog.text += $"You have <color=red>{gold}</color> gold.\n";
        }
        else if (button.text == "RECOVER")
        {
            dialog.text = $"Fully recover your HP and MP.\n";
            dialog.text += $"This will spend <color=red>40</color> gold!\n";
            dialog.text += $"You have <color=red>{gold}</color> gold.\n";
        }
    }

    private void OnLevelButtonClicked(ClickEvent evt)
    {
        int expNeed = playerBattleScript.CalculateNeedEXPToLevelUp();
        if (expNeed <= playerBattleScript.exp)
        {
            playerBattleScript.LevelUp();
            int level = playerBattleScript.level;
            dialog.text = $"Level Up! You are now at Level {level}";
        } else
        {
            dialog.text = $"Not enough exp.";
        }
    }

    private void OnDialogBackGroundClicked(ClickEvent evt)
    {
        indexOfShownText++;
        string[] toBeShown = texts.ToArray();
        if (indexOfShownText >= toBeShown.Length)
        {
            indexOfShownText = 0;
            dialog.text = texts.ToArray()[indexOfShownText];
            OnCloseButtonClicked(evt);
        } else
        {
            dialog.text = texts.ToArray()[indexOfShownText];
        }
    }

    private void OnCloseButtonClicked(ClickEvent evt)
    {
        Hide(uiDocument.rootVisualElement);
    }

    void Hide(VisualElement element)
    {
        element.style.visibility = Visibility.Hidden;
        foreach (var child in element.Children())
        {
            // Recursively hide all elements
            Hide(child);
        }
    }
    void Show(VisualElement element)
    {
        element.style.visibility = Visibility.Visible;
        foreach (var child in element.Children())
        {
            // Recursively show all elements
            Show(child);
        }
    }

    public void BeginConversation()
    {
        Show(uiDocument.rootVisualElement);
        if (!isTradable)
        {
            // Not showing action buttons
            Hide(leftActions);
        }
        
    }

    void OnEnable()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (whoIsTalkingTo != null)
        {
            if (whoIsTalkingTo.transform.position.x < transform.position.x)
            {
                faceLeft = true;
            }
            else
            {
                faceLeft = false;

            }
            animator.SetBool("b_faceLeft", faceLeft);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                whoIsTalkingTo = collision.gameObject;
                playerBattleScript = whoIsTalkingTo.GetComponent<BattleScript>();
                collision.gameObject.GetComponent<PlayerController>().ableToInteract = true;
                collision.gameObject.GetComponent<PlayerController>().InteractObject = gameObject;
                hint.SetActive(true);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                whoIsTalkingTo = null;
                playerBattleScript = null;
                collision.gameObject.GetComponent<PlayerController>().ableToInteract = false;
                collision.gameObject.GetComponent<PlayerController>().InteractObject = null;
                hint.SetActive(false);
                indexOfShownText = 0;
                dialog.text = texts.ToArray()[indexOfShownText];
                Hide(uiDocument.rootVisualElement);
            }
        }
    }

}
