using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance { get; private set; }

    // Data to be passed between scenes
    public GameObject player;
    public GameObject enemy;

    [SerializeField] List<GameObject> allObjects;    // ALL objects in the mapScene;

    // Keep track of the current and previous scene
    [SerializeField] string currentScene = "";
    [SerializeField] string nextScene = "";


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

    public void startGame()
    {
        nextScene = "MapScene_1";
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive).completed += OnMapSceneLoaded;
        currentScene = nextScene;
        nextScene = "";
    }

    internal void LoadBattleScene(GameObject player, GameObject enemy)
    {
        instance.player = player;
        instance.enemy = enemy;

        nextScene = "BattleScene";
        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive).completed += OnBattleSceneLoaded;

    }

    void OnMapSceneLoaded(AsyncOperation asyncOperation)
    {
        // Get all root objects in the map scene
        allObjects = new List<GameObject>(SceneManager.GetSceneByName("MapScene_1").GetRootGameObjects());
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

    internal void LoadMapScene(BattleScript playerCopy)
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
            obj.SetActive(true);
        }
    }
}
