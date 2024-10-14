using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI dialogueText;

    private BattleScript battleScript;


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

        battleScript = GetComponent<BattleScript>();  


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
        
        //CompositeCollider2D platform = collision.gameObject.GetComponent<CompositeCollider2D>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Player collides with the enemy
            Debug.Log("Collide with the enemy");
            SceneManagerScript.instance.LoadBattleScene(gameObject, collision.gameObject);
        }

        float groundCheckThreshold = 0.1f;

        // 遍历所有接触点
        foreach (ContactPoint2D contact in collision.contacts)
    {
        // 判断接触点是否在玩家的底部附近
        if (contact.point.y < playerRb.position.y)
        {
            // 重置跳跃状态以允许再次跳跃
            isJump = false;
            animator.SetBool("b_isJump", isJump);
            animator.SetFloat("f_Move_Y", 0);
            break;  // 找到一个有效的接触点后就可以退出循环
        }
    }

        if (collision.gameObject.CompareTag("Statue"))
        {
            // When the player collides with the statue, display the dialogue panel
            dialoguePanel.SetActive(true);
            canMove = false;
        }
    }

    // Detect when player falls to the bottom boundary
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BottomBoundary"))
        {
            Debug.Log("Player fell out of bounds");

            // reposition
            playerRb.position = new Vector2(-78, -10); 

            if (battleScript != null)
            {
                battleScript.TakeDamage(10); // HP -10
            }
        }
    }

    void Jump(InputAction.CallbackContext callbackContext)//
    {
        if (!isJump)
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

        if (dialogueText != null)
        {
            dialogueText.text = "Praying.... The ladder is spinning....";
        }

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

        StartCoroutine(HideDialogueAfterDelay(3.0f));
}

    // Coroutine to hide dialogue after a delay
    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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
