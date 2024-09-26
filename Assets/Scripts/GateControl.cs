using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    private GameObject player;
    private Color gateColor;
    private Color playerColor;
    private float speed = 8;
    private float moveDistance = 2;
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerColor =  player.GetComponent<Renderer>().material.color;
        gateColor = GetComponent<Renderer>().material.color;

        startPosition = transform.position;
        endPosition = startPosition + transform.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        playerColor = player.GetComponent<Renderer>().material.color;
        Vector3 targetPosition;

        if (ContainsColor())
        {
            targetPosition = endPosition;

        } else
        {
            targetPosition = startPosition;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    bool ContainsColor()
    {
        if (playerColor.r - gateColor.r >= 0 &&
            playerColor.g - gateColor.g >= 0 &&
            playerColor.b - gateColor.b >= 0)
        {
            return true;
        }
         else
        {
            return false; 
        }
    }
}
