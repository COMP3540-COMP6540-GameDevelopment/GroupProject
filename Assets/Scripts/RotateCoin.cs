using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    private GameManager gameManager;

    private float rotationSpeed = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        { 
            transform.Rotate(Vector3.forward, rotationSpeed);
        }
    }
}
