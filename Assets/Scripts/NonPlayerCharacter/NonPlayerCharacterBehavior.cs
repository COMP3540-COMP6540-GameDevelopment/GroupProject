using System;
using System.Collections;
using System.Collections.Generic;
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
    //[SerializeField] string firstText;
    Button closeButton;
    public GameObject whoIsTalkingTo;

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
        dialog = dialogBackGround.Q<Label>("Dialog");
        indexOfShownText = 0;
        dialog.text = texts.ToArray()[indexOfShownText];

        closeButton = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions").Q<Button>("CloseButton");
        closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        dialogBackGround.RegisterCallback<ClickEvent>(OnDialogBackGroundClicked);

        Hide(uiDocument.rootVisualElement);
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

    public void Hide(VisualElement element)
    {
        element.style.visibility = Visibility.Hidden;
        foreach (var child in element.Children())
        {
            // Recursively hide all elements
            Hide(child);
        }
    }
    public void Show(VisualElement element)
    {
        element.style.visibility = Visibility.Visible;
        foreach (var child in element.Children())
        {
            // Recursively show all elements
            Show(child);
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
                collision.gameObject.GetComponent<PlayerController>().ableToInteract = false;
                collision.gameObject.GetComponent<PlayerController>().InteractObject = null;
                hint.SetActive(false);
                Hide(uiDocument.rootVisualElement);
            }
        }
    }

}
