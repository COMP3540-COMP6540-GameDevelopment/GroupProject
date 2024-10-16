using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    // public bool isGameActive;
    // private float time = 999;
    // public TextMeshProUGUI timeText;
    public GameObject escMenuCanvas; // Assign the ESC menu Canvas in the Inspector
    private bool isMenuActive = false;
    // Start is called before the first frame update
    void Start()
    {
        // isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
         // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        // if (time > 0 && isGameActive)
        // {
        //     time -= Time.deltaTime;
        //     if (time < 0)
        //     {
        //         time = 0;
        //     }
        //     UpdateTime();
        // }
        // else if (time <= 0)
        // {
        //     Debug.Log("Time's up!");
        // }
    }
    // Method to toggle the ESC menu visibility
    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        escMenuCanvas.SetActive(isMenuActive);

        // Pause or resume the game when the menu is active
        if (isMenuActive)
        {
            Time.timeScale = 0; // Pause the game
        }
        else
        {
            Time.timeScale = 1; // Resume the game
        }
    }

    // Method to explicitly close the ESC menu (for buttons)
    public void CloseMenu()
    {
        isMenuActive = false;
        escMenuCanvas.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }
    // public void UpdateTime(){
    //     int seconds = Mathf.FloorToInt(time);
    //     timeText.text = "Time: " + seconds;
    // }
}
