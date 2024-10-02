using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatGroundMovement : MonoBehaviour
{
    private GameManager gameManager;
    
    public float moveDistance;
    private float speed = 2;
    private float startPos;
    public bool moveLeft;
    public bool VerticalMode;
    private Vector3 moveDirection1;
    private Vector3 moveDirection2;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        startPos = transform.position.x;
        moveDirection1 = Vector3.left;
        moveDirection2 = Vector3.right;
        if (VerticalMode)
        {
            startPos = transform.position.y;
            moveDirection1 = Vector3.down;
            moveDirection2 = Vector3.up;

        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentPos = transform.position.x;
        if (VerticalMode)
        {
            currentPos = transform.position.y;
        }
               
        if (gameManager.isGameActive)
        {
            // Move left
            if (moveLeft)
            {
                transform.Translate(moveDirection1 * speed * Time.deltaTime);

                if (currentPos + moveDistance <= startPos)
                {
                    moveLeft = false;
                }
            }
            // Move right
            else
            {
                transform.Translate(moveDirection2 * speed * Time.deltaTime);

                if (currentPos - moveDistance >= startPos)
                {
                    moveLeft = true;
                }
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            collision.gameObject.GetComponent<PlayerController>().isOnGround = false;
        }
    }
}
