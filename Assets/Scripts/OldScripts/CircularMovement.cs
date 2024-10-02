using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform orbitCenter;
    public float orbitRadius;
    public float orbitSpeed;
    public float angle;
    public bool isClockwise;

    private void Start()
    {
        orbitRadius = orbitCenter.transform.position.y - transform.position.y;
    }
    void Update()
    {
        // Update the angle based on the speed and time
        angle = (angle + orbitSpeed * Time.deltaTime) % 360;

        // Convert angle to radians for trigonometric functions
        float radianAngle = Mathf.Deg2Rad * angle;
        float x, y;

        if (isClockwise)
        {
            x = orbitCenter.position.x + Mathf.Cos(radianAngle) * orbitRadius;
            y = orbitCenter.position.y + Mathf.Sin(radianAngle) * orbitRadius;
        }
        else
        {
            x = orbitCenter.position.x + Mathf.Sin(radianAngle) * orbitRadius;
            y = orbitCenter.position.y + Mathf.Cos(radianAngle) * orbitRadius;
        }
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
