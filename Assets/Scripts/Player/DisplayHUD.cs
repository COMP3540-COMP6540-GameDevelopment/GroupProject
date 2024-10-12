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
    VisualElement playerStatus;
    VisualElement playerhealthBar;
    VisualElement playermagicBar;
    Label playerHP_Number;
    Label playerMP_Number;
    public float currentPlayerHealthRatio = 1f;
    public float currentPlayerMagicRatio = 1f;

    VisualElement details;
    Label levelValue;
    Label damageValue;
    Label defenseValue;
    Label expValue;
    Label goldValue;

    // Variables related to player and enemy object will be displayed in this UI
    BattleScript player;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        player = gameObject.GetComponent<BattleScript>();
        
        playerStatus = uiDocument.rootVisualElement.Q<VisualElement>("Status");
        playerName = playerStatus.Q<Label>("Name");
        playerhealthBar = playerStatus.Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<VisualElement>("HP_Line");
        playermagicBar = playerStatus.Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<VisualElement>("MP_Line");
        playerHP_Number = playerStatus.Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<Label>("HP_Number");
        playerMP_Number = playerStatus.Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<Label>("MP_Number");

        details = uiDocument.rootVisualElement.Q<VisualElement>("Details");
        levelValue = details.Q<VisualElement>("Level").Q<Label>("Value");
        damageValue = details.Q<VisualElement>("Damage").Q<Label>("Value");
        defenseValue = details.Q<VisualElement>("Defense").Q<Label>("Value");
        expValue = details.Q<VisualElement>("EXP").Q<Label>("Value");
        goldValue = details.Q<VisualElement>("Gold").Q<Label>("Value");

        HideStatusPanel();
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Start();
        Show(playerStatus);
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

    public void Hide(VisualElement element)
    {
        element.style.visibility = Visibility.Hidden;
        foreach (var child in element.Children())
        {
            // Recursively hide all elements
            Hide(child);
        }
    }

    public void ShowStatusPanel()
    {
        Show(details);
    }

    public void HideStatusPanel()
    {
        Hide(details);
    }

    public void ShowTopLeftStatus()
    {
        Show(playerStatus);
    }

    public void HideTopLeftStatus()
    {
        Hide(playerStatus);
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

        levelValue.text = player.level.ToString();
        damageValue.text = player.damage.ToString();
        defenseValue.text = player.defense.ToString();
        expValue.text = player.exp.ToString();
        goldValue.text = player.gold.ToString();
    }
}
