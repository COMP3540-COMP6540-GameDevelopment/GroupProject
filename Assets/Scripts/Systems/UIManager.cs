using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject escMenuCanvas; 
    public VisualElement operatingElement;
    public static UIManager instance  { get; private set; }
    private bool isMenuActive = false;
    private bool isShow = false;
    public GameObject gameOverUI;  
    public GameObject player_ui;
    public Transform player;     
    public float minYThreshold = -28f; 

    public void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        operatingElement = root.Q<VisualElement>("Operating");

        if (operatingElement == null)
        {
            Debug.LogError("Operating element not found in HUD.uxml!");
        }
        operatingElement.style.display = DisplayStyle.None;
    }
    public void Update()
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
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        operatingElement = root.Q<VisualElement>("Operating");

        if (operatingElement == null)
        {
            Debug.LogError("Operating element not found in HUD.uxml!");
        }
        if(!isShow){
            operatingElement.style.display = DisplayStyle.None;
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

    public void TriggerGameOver()
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
    public void showOrHide(){
        isShow = !isShow;
        if (operatingElement != null)
        {
            operatingElement.style.display = isShow ? DisplayStyle.Flex: DisplayStyle.None;
        }
    }
}
