using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables related to movement on the map
    [SerializeField] InputAction moveAction;
    Rigidbody2D playerRb;
    [SerializeField] float speed;
    Vector2 move;

    // Variables related to jump
    [SerializeField] InputAction jumpAction;
    bool isJump;
    [SerializeField] float jumpVelocity;

    // Variables related to animation
    Animator animator;
    float moveDirection = 1;
    float moveSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Movement
        moveAction.Enable();
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("f_Move_X", moveDirection);
        animator.SetFloat("f_Speed", moveSpeed);

        //Jump
        jumpAction.Enable();
        isJump = false;

    }

    // Update is called once per frame
    void Update()
    {
        // Related to movement
        move = moveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0))
        {
            moveDirection = move.x;
            animator.SetFloat("f_Move_X", moveDirection);
            animator.SetFloat("f_Speed", speed);
        }
        else
        {
            animator.SetFloat("f_Speed", 0f);
        }

        // Related to jump
        jumpAction.performed += Jump;   // bind Jump() method to this action
        if (isJump)
        {
            animator.SetFloat("f_Move_Y", playerRb.velocity.y);
            if (Mathf.Approximately(playerRb.velocity.y, 0))
            {
                isJump = false;
                animator.SetBool("b_isJump", isJump);
                animator.SetFloat("f_Move_Y", 0);
            }
        }
    }

    private void FixedUpdate()
    {
        // Related to movement
        Vector2 position = playerRb.position + speed * Time.deltaTime * move;
        playerRb.position = position;
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CompositeCollider2D platform = collision.gameObject.GetComponent<CompositeCollider2D>();

        if (platform != null)
        {
            // Player collides with the platform
            jumpAction.Enable();
            isJump = false;
            animator.SetBool("b_isJump", isJump);
            animator.SetFloat("f_Move_Y", 0);
        }
    }

    void Jump(InputAction.CallbackContext callbackContext)
    {
        if (isJump)
        {
            // Double jump
            jumpAction.Disable();
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
            animator.SetTrigger("t_DoubleJump");
        } 
        else
        {
            isJump = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpVelocity);
            animator.SetBool("b_isJump", isJump);
        }

    }
    
}
