using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderRotator : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation (degrees per second)
    private bool shouldRotate = false;
    private Quaternion targetRotation; // Target rotation

    private void Start()
    {
        // Set the initial target rotation to the current rotation
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (shouldRotate)
        {
            // Smoothly rotate the ladder to the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // If the ladder has reached the target rotation, stop rotating
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                shouldRotate = false;
            }
        }
    }

    // Method to initiate rotation of the ladder
    public void RotateLadder()
    {
        // Set the target rotation to 90 degrees around the Z-axis from the current rotation
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
        shouldRotate = true; // Start rotating the ladder
    }
}

