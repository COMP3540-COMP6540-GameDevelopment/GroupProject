using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] GameObject hint;
    [SerializeField] GameObject potionPrefab;  // ҩˮԤ��
    [SerializeField] Transform potionSpawnPoint;  // ҩˮ����λ��
    UIDocument uiDocument;
    [SerializeField] List<string> texts;
    [SerializeField] TextMeshProUGUI statusText;

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

        chestAnimator = GetComponent<Animator>();
        hint.SetActive(false);

        dialogBackGround = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Dialog_Background");
        dialogBackGround.RegisterCallback<ClickEvent>(OnDialogBackGroundClicked);

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
        statusText.gameObject.SetActive(false);  // ȷ���ı���ʼʱ�����ص�
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
            StartCoroutine(WaitForChestOpenAnimation());
            ShowStatusText(); // ֱ����ʾԤ���ı�
            Hide(uiDocument.rootVisualElement); // �رնԻ���
        }
        else
        {
            dialog.text = "box cannot be open"; // ��ʾ��������ѱ���
        }
    }
    IEnumerator WaitForChestOpenAnimation()
    {
        yield return new WaitForSeconds(1f); 
        StartCoroutine(SpawnPotion());
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
        // �������ƴ�����Ӧ���͵�ҩˮ
        string potionType = potionPrefab.name;
        GameObject potion = Instantiate(potionPrefab, potionSpawnPoint.position, Quaternion.identity);
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        BattleScript playerBattleScript = playerTransform.GetComponent<BattleScript>();

        while (Vector3.Distance(potion.transform.position, playerTransform.position) > 0.0001f)
        {
            potion.transform.position = Vector3.MoveTowards(potion.transform.position, playerTransform.position, 0.5f * Time.deltaTime);
            yield return null;
        }
        Destroy(potion); // ҩˮ�������λ�ú����٣���ʾ��һ��ҩˮ
                         
        // ����ҩˮ���͵����������
        switch (potionType)
        {
            case "Potions 64x64 BG transparent BLUE_8":
                playerBattleScript.RecoverMP(10); // ���� MP ֵ
                break;
            case "Potions 64x64 BG transparent GREEN_16":
                playerBattleScript.exp += 50; // ���Ӿ���ֵ
                break;
            case "Potions 64x64 BG transparent ORANGE_40":
                playerBattleScript.gold += 100; // ���ӽ��
                break;
            case "Potions 64x64 BG transparent RED_0":
                playerBattleScript.RecoverHP(20); // ���� HP ֵ
                break;
            case "Potions 64x64 BG transparent YELLOW_24":
                playerBattleScript.TakeDamage(10); // ���� HP ֵ
                break;
        }

        if (playerBattleScript.gameObject.GetComponent<DisplayHUD>() != null)
        {
            playerBattleScript.gameObject.GetComponent<DisplayHUD>().UpdateStatus();
        }
    }

    void ShowStatusText()
    {
        statusText.gameObject.SetActive(true); // ֻ��ʾ�ı�������������
        StartCoroutine(HideStatusTextAfterDelay(3.0f)); // 3��������ı�
    }

    IEnumerator HideStatusTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        statusText.gameObject.SetActive(false);
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
        Debug.Log("Begin conversation with chest.");
        Show(uiDocument.rootVisualElement);
        if (!isOpenable)
        {
            dialog.text = "box is opening";
            // Not showing action buttons
            Hide(leftActions);
        }
        else
        {
            dialog.text = "box cannot be open...";
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
