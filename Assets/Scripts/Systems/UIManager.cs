using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject escMenuCanvas; 

    private bool isMenuActive = false;
    public GameObject gameOverUI;  
    public GameObject player_ui;
    public Transform player;     
    public float minYThreshold = -28f; 
    void Update()
    {
         // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        
        if (player != null && player.position.y < minYThreshold)
        {
            TriggerGameOver();
        }
    }
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

    public void CloseMenu()
    {
        isMenuActive = false;
        escMenuCanvas.SetActive(false);
        Time.timeScale = 1; 
    }

    void TriggerGameOver()
    {
        player_ui.SetActive(false);
        gameOverUI.SetActive(true); 
        Time.timeScale = 0f;         
    }

    public void RestartGame()
    {
        if (SceneManagerScript.instance != null)
        {
            SceneManagerScript.instance.Restart();
            Time.timeScale = 1; 
        }
        else
        {
            Debug.LogError("SceneManagerScript instance is not available.");
        }
    }
    
    public void BackHome()
    {
        if (SceneManagerScript.instance != null)
        {
            SceneManagerScript.instance.backhome();
            
        }
        else
        {
            Debug.LogError("SceneManagerScript instance is not available.");
        }


    }

    public void escBackHome(){
         if (SceneManagerScript.instance != null)
        {
            SceneManagerScript.instance.eschome();
            
        }
        else
        {
            Debug.LogError("SceneManagerScript instance is not available.");
        }
    }
}
