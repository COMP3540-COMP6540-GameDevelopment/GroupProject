using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] GameObject hint;
    [SerializeField] GameObject potionPrefab;  // 药水预设
    [SerializeField] Transform potionSpawnPoint;  // 药水生成位置
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
        statusText.gameObject.SetActive(false);  // 确保文本开始时是隐藏的
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
            isOpenable = false; // 确保箱子只能被打开一次
            chestAnimator.SetBool("Open", true); // 触发开箱动画 
            StartCoroutine(WaitForChestOpenAnimation());
            ShowStatusText(); // 直接显示预设文本
            Hide(uiDocument.rootVisualElement); // 关闭对话框
        }
        else
        {
            dialog.text = "box cannot be open"; // 提示玩家箱子已被打开
        }
    }
    IEnumerator WaitForChestOpenAnimation()
    {
        yield return new WaitForSeconds(1f); 
        StartCoroutine(SpawnPotion());
    }

    private void OnNoButtonClicked(ClickEvent evt)
    {
        OnCloseButtonClicked(evt); // 关闭对话窗
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
        // 根据名称创建对应类型的药水
        string potionType = potionPrefab.name;
        GameObject potion = Instantiate(potionPrefab, potionSpawnPoint.position, Quaternion.identity);
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        BattleScript playerBattleScript = playerTransform.GetComponent<BattleScript>();

        while (Vector3.Distance(potion.transform.position, playerTransform.position) > 0.0001f)
        {
            potion.transform.position = Vector3.MoveTowards(potion.transform.position, playerTransform.position, 0.5f * Time.deltaTime);
            yield return null;
        }
        Destroy(potion); // 药水到达玩家位置后销毁，表示玩家获得药水
                         
        // 根据药水类型调整玩家属性
        switch (potionType)
        {
            case "Potions 64x64 BG transparent BLUE_8":
                playerBattleScript.RecoverMP(10); // 增加 MP 值
                break;
            case "Potions 64x64 BG transparent GREEN_16":
                playerBattleScript.exp += 50; // 增加经验值
                break;
            case "Potions 64x64 BG transparent ORANGE_40":
                playerBattleScript.gold += 100; // 增加金币
                break;
            case "Potions 64x64 BG transparent RED_0":
                playerBattleScript.RecoverHP(20); // 增加 HP 值
                break;
            case "Potions 64x64 BG transparent YELLOW_24":
                playerBattleScript.TakeDamage(10); // 减少 HP 值
                break;
        }

        if (playerBattleScript.gameObject.GetComponent<DisplayHUD>() != null)
        {
            playerBattleScript.gameObject.GetComponent<DisplayHUD>().UpdateStatus();
        }
    }

    void ShowStatusText()
    {
        statusText.gameObject.SetActive(true); // 只显示文本，不更改内容
        StartCoroutine(HideStatusTextAfterDelay(3.0f)); // 3秒后隐藏文本
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
            playerNearby = true;  // 设置玩家靠近标志为true
            collision.gameObject.GetComponent<PlayerController>().ableToInteract = true;
            collision.gameObject.GetComponent<PlayerController>().InteractObject = gameObject;
            hint.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerNearby = false;  // 设置玩家靠近标志为false
            collision.gameObject.GetComponent<PlayerController>().ableToInteract = false;
            collision.gameObject.GetComponent<PlayerController>().InteractObject = null;
            hint.SetActive(false);
            indexOfShownText = 0;
            dialog.text = texts.ToArray()[indexOfShownText];
            Hide(uiDocument.rootVisualElement);
        }
    }

}
