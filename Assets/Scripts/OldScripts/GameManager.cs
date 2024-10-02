using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI boostText;
    public GameObject titleScreen;
    public GameObject HintTexts;
    public Button restartButton;
    private GameObject player;
    private GameObject others;

    private int coinValue;


    private int time;
    public bool isGameActive;
    public int gameDifficulty;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        others = GameObject.Find("Others");
    }

    // Update coins
    public void UpdateCoins()
    {
        coinValue += 1;
        coinsText.text = "Coins: " + coinValue;
    }
    IEnumerator UpdateTime()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            if (time == 0)
            {
                GameOver(false);
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        timeText.text = "Time: " + time;
        if (player.transform.position.y < -10)
        {
            GameOver(false);
        }
        int r = (int)player.GetComponent<Renderer>().material.color.r;
        int g = (int)player.GetComponent<Renderer>().material.color.g;
        int b = (int)player.GetComponent<Renderer>().material.color.b;
        if (r != 1 || g != 1 || b != 1)
        {
            string text = "Find RGB colors\r\n" + "(" + r + ", " + g + ", " + b + ")";
            UpdateMission(text);
        } else
        {
            UpdateMission("Jump to the right!");
        }
    }
    public void UpdateMission(string text)
    {
        missionText.text = text;
    }
    public void StartGame(int difficulty)
    {
        gameDifficulty = difficulty;
        coinValue = 0;
        switch (difficulty)
        {
            case 1:
                time = 999;
                break;
            case 2:
                time = 480;
                break;
            case 3:
                time = 240; 
                break;

            default:
                break;
        }
        isGameActive = true;
        UpdateMission("Jump to the right!");
        titleScreen.SetActive(false);
        coinsText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        missionText.gameObject.SetActive(true);
        HintTexts.SetActive(true);
        StartCoroutine(UpdateTime());

    }
    public void GameOver(bool isClear)
    {
        if (isClear)
        {
            int finalScore = coinValue * 357 + (int)(time * 0.5f);
            gameOverText.text = ("Congratulations!");
            scoreText.text = "Final Score: " + finalScore;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        } else
        {
            others.SetActive(false);
            scoreText.text = "You have collected " + coinValue + " coins";
            player.transform.position = new Vector3(player.transform.position.x, -10f, player.transform.position.z);
        }
        others.SetActive(false);
        player.GetComponent<Renderer>().enabled = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        
        scoreText.gameObject.SetActive(true);
        missionText.gameObject.SetActive(false);
        boostText.gameObject.SetActive(false);
        isGameActive = false;

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
