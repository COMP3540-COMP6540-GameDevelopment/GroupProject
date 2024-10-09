using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    // Variables related to movement on the map
    [SerializeField] InputAction moveAction;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float speed;
    Vector2 move;

    // Variables related to jump
    [SerializeField] InputAction jumpAction;
    bool isJump;
    [SerializeField] float jumpVelocity;

    // Variables related to animation
    Animator animator;
    [SerializeField] float moveDirection;
    float moveSpeed;
    public bool isBattle = false;

    public GameObject dialoguePanel; 
    public Button option1Button;     
    public Button option2Button; 

    private void Awake()
    {
        moveDirection = 1;
        moveSpeed = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Movement
        moveAction.Enable();
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if(!isBattle)
        {
            animator.SetFloat("f_Move_X", moveDirection);
            animator.SetFloat("f_Speed", moveSpeed);
        }
      

        //Jump
        jumpAction.Enable();
        jumpAction.performed += Jump;   // bind Jump() method to this action
        isJump = false;

        // Initially hide the dialogue panel
        dialoguePanel.SetActive(false);

        // Add listeners to the buttons
        option1Button.onClick.AddListener(OnOption1Selected);
        option2Button.onClick.AddListener(OnOption2Selected);

    }
    // Called when reactivate
    void OnEnable()
    {
        // Movement
        //moveAction.Enable();
        //playerRb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //if (!isBattle)
        //{
        //    animator.SetFloat("f_Move_X", moveDirection);
        //    animator.SetFloat("f_Speed", moveSpeed);
        //}

        ////Jump
        //jumpAction.Enable();
        //isJump = false;
        Start();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        jumpAction.performed -= Jump;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
        // Related to movement
        move = moveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0))
        {
            moveDirection = move.x;
            if (!isBattle)
            {
                animator.SetFloat("f_Move_X", moveDirection);
                animator.SetFloat("f_Speed", speed);
            }
        }
        else
        {
            animator.SetFloat("f_Speed", 0f);
        }

        // Related to jump
        
        if (isJump)
        {
            if (!isBattle)
            {
                animator.SetFloat("f_Move_Y", playerRb.velocity.y);
            }
            if (Mathf.Approximately(playerRb.velocity.y, 0))
            {
                isJump = false;
                if (!isBattle)
                {
                    animator.SetBool("b_isJump", isJump);
                }
                animator.SetFloat("f_Move_Y", 0);
            }
        }
        // Boundary check
        Vector2 playerPosition = playerRb.position;

     
    }
    }

    private void FixedUpdate()
    {
        if (canMove){
        // Related to movement
        Vector2 position = playerRb.position + speed * Time.deltaTime * move;
        playerRb.position = position;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        CompositeCollider2D platform = collision.gameObject.GetComponent<CompositeCollider2D>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Player collides with the enemy
            Debug.Log("Collide with the enemy");
            SceneManagerScript.instance.LoadBattleScene(gameObject, collision.gameObject);
        }

        if (platform != null)
        {
            // Player collides with the platform
            jumpAction.Enable();
            isJump = false;
            animator.SetBool("b_isJump", isJump);
            animator.SetFloat("f_Move_Y", 0);
        }

        if (collision.gameObject.CompareTag("Statue"))
        {
            // When the player collides with the statue, display the dialogue panel
            dialoguePanel.SetActive(true);
            canMove = false;
        }
    }

    void Jump(InputAction.CallbackContext callbackContext)
    {
        if (isJump)
        {
            // Double jump
            jumpAction.Disable();
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
            if (!isBattle)
            {
                animator.SetTrigger("t_DoubleJump");
            }
        } 
        else
        {
            isJump = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
            if (!isBattle)
            {
                animator.SetBool("b_isJump", isJump);
            }
        }

    }
    
    // When option 1 (Pray) is selected
    private void OnOption1Selected()
    {
        Debug.Log("Player chose to pray, the ladder rotates.");

        // Trigger the rotation of a different ladder
        GameObject ladderPivot = GameObject.Find("LadderPivot"); // Assume the pivot object's name is "LadderPivot"
        if (ladderPivot != null)
        {
          LadderRotator ladderRotator = ladderPivot.GetComponent<LadderRotator>();
          if (ladderRotator != null)
          {
             ladderRotator.RotateLadder(); // Trigger the ladder rotation
            }
    }

        // Hide the dialogue panel and allow player movement
        dialoguePanel.SetActive(false);
        canMove = true; // Re-enable player movement and jumping
}


    // When option 2 (Leave) is selected
    private void OnOption2Selected()
    {
        // Player chose to leave, simply hide the dialogue panel
        Debug.Log("Player chose to leave, dialogue panel disappears.");
        dialoguePanel.SetActive(false);
        canMove = true; 
    }

    // Ladder falling logic
    private void LadderFall()
    {
        // Implement the logic for making the ladder fall
        // For example, find the ladder and change its position or enable falling animation
        GameObject ladder = GameObject.Find("Ladder");
        if (ladder != null)
        {
            // Simple example: Directly change the position to simulate falling
            ladder.transform.position += new Vector3(0, -5, 0);
        }
    }
}
