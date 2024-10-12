using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayHUD : MonoBehaviour
{
    UIDocument uiDocument;

    // Variables related to player status
    Label playerName;
    VisualElement playerhealthBar;
    VisualElement playermagicBar;
    Label playerHP_Number;
    Label playerMP_Number;
    public float currentPlayerHealthRatio = 1f;
    public float currentPlayerMagicRatio = 1f;

    // Variables related to player and enemy object will be displayed in this UI
    BattleScript player;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        player = gameObject.GetComponent<BattleScript>();

        playerName = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<Label>("Name");
        playerhealthBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<VisualElement>("HP_Line");
        playermagicBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<VisualElement>("MP_Line");
        playerHP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<Label>("HP_Number");
        playerMP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<Label>("MP_Number");

    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatus();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Start();
        Show(uiDocument.rootVisualElement);
    }

    private void OnDisable()
    {
        
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


    public void UpdateStatus()
    {
        playerName.text = player.battleObjectName;
        playerHP_Number.text = $"{player.currentHP}/{player.maxHP}";
        currentPlayerHealthRatio = player.currentHP / (float)player.maxHP;
        playerhealthBar.style.width = Length.Percent(currentPlayerHealthRatio * 100.0f);
        playerMP_Number.text = $"{player.currentMP}/{player.maxMP}";
        currentPlayerMagicRatio = player.currentMP / (float)player.maxMP;
        playermagicBar.style.width = Length.Percent(currentPlayerMagicRatio * 100.0f);
    }
}
