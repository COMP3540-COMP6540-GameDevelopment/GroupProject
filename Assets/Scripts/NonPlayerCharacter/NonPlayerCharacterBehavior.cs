using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

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
        levelButton.RegisterCallback<ClickEvent>(OnLevelButtonClicked);


        levelButton.RegisterCallback<MouseEnterEvent>(DisplayButtonDescription);
        levelButton.RegisterCallback<MouseLeaveEvent>(HideButtonDescription);

        Hide(uiDocument.rootVisualElement);
    }

    private void HideButtonDescription(MouseLeaveEvent evt)
    {
        dialog.text = texts.First();
    }

    private void DisplayButtonDescription(MouseEnterEvent evt)
    {
        Button button = (Button)evt.target;  // Get the button from the event
        if (button.text == "LEVEL")
        {
            int expNeed = playerBattleScript.CalculateNeedEXPToLevelUp();
            int exp = playerBattleScript.exp;
            int level = playerBattleScript.level;
            dialog.text = $"Level up and  increase Max HP, MP by <color=red>100</color>,\nDamage, Defense by <color=red>2</color>.\n";
            dialog.text += $"You are Level {level}. This will spend <color=red>{expNeed}</color> exp!\n";
            dialog.text += $"You have <color=red>{exp}</color> exp.\n";
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
