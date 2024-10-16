using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }


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

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("DangerZone"))
    {
        // Player enter danger zone
        Debug.Log("Player entered a danger zone!");

        if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }

        // reduce HP
        BattleScript playerBattleScript = GetComponent<BattleScript>();
        if (playerBattleScript != null)
        {
            int damage = 10; // HP-10
            playerBattleScript.TakeDamage(damage);
            Debug.Log("Player took " + damage + " damage. Current HP: " + playerBattleScript.currentHP);
        }

        // Restart game
        StartCoroutine(RestartAfterDelay(2.0f));
    }
}

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide dialog box after delay
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // Restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    



}
