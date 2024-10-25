using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeZone : MonoBehaviour
{
    public Scene3LadderController ladder; 
    private int touchingObjectsCount = 0; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check the object collide with is "Player" or "Box"
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            Debug.Log("Object Entered: " + other.name); 
            touchingObjectsCount++;
            ladder.MoveUp(); // Ladder rise
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check leaving object is "Player" and "Box"
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            Debug.Log("Object Exited: " + other.name); 
            touchingObjectsCount--;

            // When no object collide with, ladder fall down
            if (touchingObjectsCount <= 0)
            {
                touchingObjectsCount = 0; 
                ladder.MoveDown(); // Ladder falls
            }
        }
    }
}


