using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject player_ui;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter called. Collider detected: " + other.name);
        if (other.CompareTag("Player"))
        {
            if (winCanvas != null)
            {
                player_ui.SetActive(false);
                winCanvas.SetActive(true);
                Debug.Log("Game Won! Displaying Win Canvas.");
            }
            else
            {
                Debug.LogError("Win Canvas is not assigned.");
            }
        }
    }
}
