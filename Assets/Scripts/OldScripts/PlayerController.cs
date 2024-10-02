using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private CameraZoom cameraZoom;
    private GameObject playerCamera;
    private GameObject backGround;

    private float speed = 5;
    private float boostSpeed = 10;

    private Rigidbody playerRb;

    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public TextMeshProUGUI boostText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerCamera = GameObject.Find("Main Camera");
        cameraZoom = playerCamera.GetComponent<CameraZoom>();
        backGround = GameObject.Find("BackGround");


        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector3.right * boostSpeed * horizontalInput * Time.deltaTime);
                boostText.gameObject.SetActive(true);

            } else
            {
                transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
                boostText.gameObject.SetActive(false);
            }
            // Move code
            
            
            

            // Jump code
            if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Float"))
        {
            isOnGround = true;
        }
    }
}
