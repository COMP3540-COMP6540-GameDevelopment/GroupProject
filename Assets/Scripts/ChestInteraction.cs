using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] GameObject hint;
    [SerializeField] GameObject potionPrefab;  // ҩˮԤ��
    [SerializeField] Transform potionSpawnPoint;  // ҩˮ����λ��
    UIDocument uiDocument;
    [SerializeField] List<string> texts;

    Rigidbody2D rb;

    // Variables related to conversation dialog
    
    Label dialog;
    VisualElement dialogBackGround;
    [Tooltip("The text will be shown in the conversation from up to down")]
    [SerializeField] int indexOfShownText;
    Button closeButton;
    BattleScript playerBattleScript;
    private Animator chestAnimator;
    public bool isOpenable = true;
    VisualElement leftActions;
    Button YesButton;
    Button NoButton;
    private bool playerNearby = false;
    private bool chestOpened = false;

    // Variables related to Animation
    [SerializeField] bool faceLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component is not attached to the GameObject.");
        }

        chestAnimator = GetComponent<Animator>();
        hint.SetActive(false);

        dialogBackGround = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Dialog_Background");
        dialogBackGround.RegisterCallback<ClickEvent>(OnDialogBackGroundClicked);

        dialog = dialogBackGround.Q<Label>("Dialog");
        indexOfShownText = 0;
        dialog.text = texts.ToArray()[indexOfShownText];

        closeButton = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions").Q<Button>("CloseButton");
        closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        leftActions = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions").Q<VisualElement>("Left");

        NoButton = leftActions.Q<Button>("Action2");

        if (isOpenable)
        {
            YesButton = leftActions.Q<Button>("Action1");
            YesButton.RegisterCallback<ClickEvent>(OnYesButtonClicked);
            NoButton.RegisterCallback<ClickEvent>(OnNoButtonClicked);
        }

        Hide(uiDocument.rootVisualElement);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            BeginConversation();
        }
    }

    private void OnYesButtonClicked(ClickEvent evt)
    {
        if (isOpenable)
        {
            isOpenable = false; // ȷ������ֻ�ܱ���һ��
            chestAnimator.SetBool("Open", true); // �������䶯�� 
            StartCoroutine(SpawnPotion());
        }
        else
        {
            dialog.text = "�����Ѵ�"; // ��ʾ��������ѱ���
        }
    }

    private void OnNoButtonClicked(ClickEvent evt)
    {
        OnCloseButtonClicked(evt); // �رնԻ���
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
        }
        else
        {
            dialog.text = texts.ToArray()[indexOfShownText];
        }
    }

    private void OnCloseButtonClicked(ClickEvent evt)
    {
        Hide(uiDocument.rootVisualElement);
    }

    IEnumerator SpawnPotion()
    {
        GameObject potion = Instantiate(potionPrefab, potionSpawnPoint.position, Quaternion.identity);
        while (Vector3.Distance(potion.transform.position, playerBattleScript.transform.position) > 0.1f)
        {
            potion.transform.position = Vector3.MoveTowards(potion.transform.position, playerBattleScript.transform.position, 5f * Time.deltaTime);
            yield return null;
        }
        Destroy(potion); // ҩˮ�������λ�ú����٣���ʾ��һ��ҩˮ
                         // ���������������һ��ҩˮ���߼���������������ֵ��
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
        Debug.Log("Showing element: " + element.name);
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
        if (!isOpenable)
        {
            // Not showing action buttons
            Hide(leftActions);
        }

    }

    void OnEnable()
    {
        Start();
    }
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerNearby = true;  // ������ҿ�����־Ϊtrue
            collision.gameObject.GetComponent<PlayerController>().ableToInteract = true;
            collision.gameObject.GetComponent<PlayerController>().InteractObject = gameObject;
            hint.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerNearby = false;  // ������ҿ�����־Ϊfalse
            collision.gameObject.GetComponent<PlayerController>().ableToInteract = false;
            collision.gameObject.GetComponent<PlayerController>().InteractObject = null;
            hint.SetActive(false);
            indexOfShownText = 0;
            dialog.text = texts.ToArray()[indexOfShownText];
            Hide(uiDocument.rootVisualElement);
        }
    }

}
