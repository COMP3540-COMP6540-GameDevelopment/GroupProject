using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public float riseSpeed = 1f; // Speed of ascending
    public float fallSpeed = 2f; // Speed of descending
    public float maxHeight = 0f; // Maximum height the ladder can rise
    private bool playerOnLadder = false;
    private bool shouldFall = false;

    private Vector3 initialPosition; // Initial position of the ladder

    private void Start()
    {
        // Record the initial position of the ladder
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (playerOnLadder && transform.position.y < maxHeight)
        {
            // Ladder slowly rises until it reaches maxHeight
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
        else if (shouldFall)
        {
            // Ladder slowly falls back to its initial position
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, fallSpeed * Time.deltaTime);

            // If the ladder reaches the initial position, stop falling
            if (transform.position == initialPosition)
            {
                shouldFall = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player is on the ladder
            playerOnLadder = true;
            shouldFall = false; // Stop falling
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player leaves the ladder, ladder starts to fall
            playerOnLadder = false;
            shouldFall = true; // Mark that the ladder should fall
        }
    }
}
