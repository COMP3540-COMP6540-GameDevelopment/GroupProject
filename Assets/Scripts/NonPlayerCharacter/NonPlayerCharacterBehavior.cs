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
    [Tooltip("This text will be firstly shown in the conversation")]
    [SerializeField] string firstText;

    // Variables related to Animation
    Animator animator;
    [SerializeField] bool faceLeft;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        uiDocument = GetComponent<UIDocument>();
        animator = GetComponent<Animator>();

        hint.SetActive(false);
        dialog = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Dialog_Background").Q<Label>("Dialog");
        dialog.text = firstText;
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
        // Physics2D.Raycast(Ray origin, Ray direction, Ray distance, Layer mask)
        RaycastHit2D hitLeft = Physics2D.Raycast(rb.position, Vector2.left, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D hitRight = Physics2D.Raycast(rb.position, Vector2.right, 1f, LayerMask.GetMask("Player"));
        if (hitLeft.collider != null)
        {
            hint.SetActive(true);
            faceLeft = true;
            animator.SetBool("b_faceLeft", true);

        } else if (hitRight.collider != null)
        {
            hint.SetActive(true);
            faceLeft = false;
            animator.SetBool("b_faceLeft", false);
        }
        else
        {
            hint.SetActive(false);
            Hide(uiDocument.rootVisualElement);
        }
    }


}
