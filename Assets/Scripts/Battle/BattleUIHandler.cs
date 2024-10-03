using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUIHandler : MonoBehaviour
{
    public static BattleUIHandler instance { get; private set; }

    
    Label playerName;
    VisualElement playerhealthBar;
    VisualElement playermagicBar;
    Label playerHP_Number;
    Label playerMP_Number;

    
    Label enemyName;
    VisualElement enemyhealthBar;
    VisualElement enemymagicBar;
    Label enemyHP_Number;
    Label enemyMP_Number;

    VisualElement actions;
    Label dialog;

    BattleScript player;
    BattleScript enemy;

    public float currentPlayerHealthRatio = 0.2f;
    public float currentPlayerMagicRatio = 0.2f;
    public float currentEnemyHealthRatio = 0.2f;
    public float currentEnemyMagicRatio = 0.2f;


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

        enemyHP_Number.text = $"{enemy.currentHP}/{enemy.maxHP}";
        currentEnemyHealthRatio = enemy.currentHP / (float)enemy.maxHP;
        enemyhealthBar.style.width = Length.Percent(currentEnemyHealthRatio * 100.0f);
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
