using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HDROutputUtils;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance { get; private set; }

    // Data to be passed between scenes
    public GameObject player;
    public GameObject enemy;

    [SerializeField] List<GameObject> allObjects;    // ALL objects in the mapScene;

    // Keep track of the current and previous scene
    public string currentScene = "";
    public string nextScene = "";

    // Variables related to how to load map
    public bool isFromRightToLeft;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        transform.SetParent(null, false);
        // Make sure SceneManager persists across scene changes
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (nextScene != "" && nextScene != "BattleScene")
        {
            LoadMap(isFromRightToLeft);
        }
    }

    void LoadMap(bool isFromRightToLeft)
    {
        GameObject currentPlayer = null;
        if (currentScene != "")
        {
            foreach (GameObject obj in allObjects)
            {
                // Find current player
                if (obj.layer == LayerMask.NameToLayer("Player"))
                {
                    currentPlayer = obj;
                }
                obj.SetActive(false);
            }
        }
        Scene scene = SceneManager.GetSceneByName(nextScene);
        if (scene.isLoaded)
        {
            // if nextScene to load is already in the heirarchy, set all gameobjects to active
            // Update objects list
            allObjects = new List<GameObject>(scene.GetRootGameObjects());
            GameObject playerInNextScene = null;
            GameObject SpawnPosition = null;

            foreach (GameObject obj in allObjects)
            {
                
                if (obj.layer == LayerMask.NameToLayer("Player"))
                {
                    playerInNextScene = obj;
                }
                else
                {
                    if (isFromRightToLeft)
                    {
                        if (obj.name == "SpawnPositionRight")
                        {
                            SpawnPosition = obj;
                        }
                    }
                    else
                    {
                        if (obj.name == "SpawnPositionLeft")
                        {
                            SpawnPosition = obj; 
                        }
                    }
                    obj.SetActive(true);
                }
            }
            if (playerInNextScene != null)
            {
                // Need to synchronize player status
                playerInNextScene.GetComponent<BattleScript>().UpdateResults(currentPlayer.GetComponent<BattleScript>());
            }
            if (playerInNextScene != null && SpawnPosition != null)
            {

                playerInNextScene.transform.position = SpawnPosition.transform.position;
                playerInNextScene.SetActive(true);
            }
            else
            {
                Debug.Log("playerInNextScene or SpawnPosition not found");
            }
        }
        else
        {
            //nextScene to load is not in the heirarchy, load the scene
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive).completed += OnMapSceneLoaded;
        }
        
        currentScene = nextScene;
        nextScene = "";
    }

    public void startGame()
    {
        if (nextScene == "")
        {
            nextScene = "MapScene";
        }
    }

    internal void LoadBattleScene(GameObject player, GameObject enemy)
    {
        if (instance.player == null)
        {
            instance.player = player;
        }
        
        instance.enemy = enemy;

        nextScene = "BattleScene";
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive).completed += OnBattleSceneLoaded;

    }
    void OnMapSceneLoaded(AsyncOperation asyncOperation)
    {
        // Get all root objects in the map scene
        //Debug.Log($"nextScene = {nextScene}");
        //Debug.Log($"currentScene = {currentScene}");
        allObjects = new List<GameObject>(SceneManager.GetSceneByName(currentScene).GetRootGameObjects());
        // Update BattleScript
        if (player == null)
        {
            // First scene that contains a player, set reference in script
            player = allObjects.Find(obj => obj.name == "Player");
        } else
        {
            // is travelling between scenes, need to synchronize player status
            allObjects.Find(obj => obj.name == "Player").GetComponent<BattleScript>().UpdateResults(player.GetComponent<BattleScript>());
        }
    }

    void OnBattleSceneLoaded(AsyncOperation asyncOperation)
    {
        // Deactivate all root objects in the map scene
        foreach (GameObject obj in allObjects)
        {
            obj.SetActive(false);
        }
        currentScene = nextScene;
        nextScene = "";
    }

    internal void ReturnToCurrentMap(BattleScript playerCopy)
    {
        allObjects.Remove(enemy);
        Destroy(enemy);
        enemy = null;
        player.GetComponent<BattleScript>().UpdateResults(playerCopy);

        // Unload the battle scene
        SceneManager.UnloadSceneAsync(currentScene);

        // Activate all root objects in the map scene
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Player"))
            {
                obj.GetComponent<BattleScript>().UpdateResults(player.GetComponent<BattleScript>());
            }
            obj.SetActive(true);
        }
    }
}
