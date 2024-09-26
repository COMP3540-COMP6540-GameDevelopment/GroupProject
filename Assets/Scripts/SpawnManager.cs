using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameManager gameManager;
    private float spawnRangeX = 10;
    private float spawnRangeY = 10;
    private float upperBound = 6;

    public int enemyCount;


    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount == 0) 
            {
                SpawnEnemyWave(gameManager.gameDifficulty);
            }
        } else
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(item);
            }

        }
        
    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX) + player.transform.position.x;
        float yPos = Random.Range(0, spawnRangeY) + player.transform.position.y + upperBound;
        return new Vector3(xPos,yPos, 0);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
