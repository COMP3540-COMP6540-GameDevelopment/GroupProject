using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUIHandler : MonoBehaviour
{
    public static BattleUIHandler instance { get; private set; }

    // Variables related to player status
    Label playerName;
    VisualElement playerhealthBar;
    VisualElement playermagicBar;
    Label playerHP_Number;
    Label playerMP_Number;
    public float currentPlayerHealthRatio = 1f;
    public float currentPlayerMagicRatio = 1f;

    // Variables related to enemy status
    Label enemyName;
    VisualElement enemyhealthBar;
    VisualElement enemymagicBar;
    Label enemyHP_Number;
    Label enemyMP_Number;
    public float currentEnemyHealthRatio = 1f;
    public float currentEnemyMagicRatio = 1f;

    // Variables related to player actions
    public VisualElement actions;
    public List<Button> actionButtons;

    // Variables related to dialog display
    Label dialog;

    // Variables related to player and enemy object will be displayed in this UI
    BattleScript player;
    BattleScript enemy;


    private void Awake()
    {
        instance = this;

        UIDocument uiDocument = GetComponent<UIDocument>();

        playerName = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<Label>("Name");
        playerhealthBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<VisualElement>("HP_Line");
        playermagicBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<VisualElement>("MP_Line");
        playerHP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<Label>("HP_Number");
        playerMP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("PlayerStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<Label>("MP_Number");

        enemyName = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("EnemyStatus").Q<Label>("Name");
        enemyhealthBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("EnemyStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<VisualElement>("HP_Line");
        enemymagicBar = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("EnemyStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<VisualElement>("MP_Line");
        enemyHP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("EnemyStatus").Q<VisualElement>("HPBackground").Q<VisualElement>("HP_Bar").Q<Label>("HP_Number");
        enemyMP_Number = uiDocument.rootVisualElement.Q<VisualElement>("Status").Q<VisualElement>("EnemyStatus").Q<VisualElement>("MPBackground").Q<VisualElement>("MP_Bar").Q<Label>("MP_Number");

        dialog = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Dialog_Background").Q<Label>("Dialog");
        actions = uiDocument.rootVisualElement.Q<VisualElement>("Dialog_Actions").Q<VisualElement>("Actions");

        // Find all buttons
        actionButtons = new List<Button>();
        actions.Query<Button>().ForEach(button => actionButtons.Add(button));

        DisableActions();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GetPlayerEnemy(BattleScript player, BattleScript enemy)
    {
        this.player = player;
        this.enemy = enemy;

        playerName.text = player.battleObjectName;
        enemyName.text = enemy.battleObjectName;
    }

    public void UpdateStatus()
    {
        playerHP_Number.text = $"{player.currentHP}/{player.maxHP}";
        currentPlayerHealthRatio = player.currentHP / (float) player.maxHP;
        playerhealthBar.style.width = Length.Percent(currentPlayerHealthRatio * 100.0f);
        playerMP_Number.text = $"{player.currentMP}/{player.maxMP}";
        currentPlayerMagicRatio = player.currentMP / (float) player.maxMP;
        playermagicBar.style.width = Length.Percent(currentPlayerMagicRatio * 100.0f);

        enemyHP_Number.text = $"{enemy.currentHP}/{enemy.maxHP}";
        currentEnemyHealthRatio = enemy.currentHP / (float)enemy.maxHP;
        enemyhealthBar.style.width = Length.Percent(currentEnemyHealthRatio * 100.0f);
        enemyMP_Number.text = $"{enemy.currentMP}/{enemy.maxMP}";
        currentEnemyMagicRatio = enemy.currentMP / (float)enemy.maxMP;
        enemymagicBar.style.width = Length.Percent(currentEnemyMagicRatio * 100.0f);
    }

    public void UpdateDialog(string text)
    {
        dialog.text = text;
    }

    public void EnableActions()
    {
        actions.style.visibility = Visibility.Visible;
    }
    public void DisableActions()
    {
        actions.style.visibility = Visibility.Hidden;
    }
}
