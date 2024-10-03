using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUISystem : MonoBehaviour
{
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;
    public GameObject ActionUI;
    public TextMeshProUGUI dialog;
    private BattleSystem battleSystem;

    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GameObject.Find("Battle System").GetComponent<BattleSystem>();

        UpdateStatus();
        
    }

    public void UpdateStatus()
    {
        playerHPText.text = "HP:" +
            battleSystem.playerCopy.currentHP
            + "/" +
            battleSystem.playerCopy.maxHP;
        enemyHPText.text = "HP:" +
            battleSystem.enemyCopy.currentHP
            + "/" +
            battleSystem.enemyCopy.maxHP;
    }

    public void UpdateDialog(string text)
    {
        dialog.text = text;
    }
}
