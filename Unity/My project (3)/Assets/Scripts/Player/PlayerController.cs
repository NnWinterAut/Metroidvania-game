using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Movement();
        Walk();
        Jump();
        Run();
    }

    #region ---- Animator by Penelope & Winter ----
    private Rigidbody2D rigi;
    private Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region ---- Movement by Penelope & Winter ----
    public float moveSpeed = 2.0f;
    Vector2 move = new (0, 0);
    bool Onground;
    int movement_state = 0;
    bool facingRight = true;
    private void Movement()
    {
        // Get the input of the player
        move = new Vector2(Input.GetAxis("Horizontal"), 0);

        // Move the character
        rigi.velocity = new Vector2(move.x * moveSpeed * (movement_state == 2 ? runSpeedMul : 1), rigi.velocity.y);

        // Adjust the direction of the character
        if (move.x > 0 && !facingRight || move.x < 0 && facingRight)
        {
            // Inverse facing direction
            facingRight = !facingRight; 
            transform.Rotate(0f, 180f, 0f);
        }
    }
    #endregion

    #region ---- Grounding by Penelope & Winter ----
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Grouding(collision, false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Grouding(collision, false);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Grouding(collision, true);
    }
    private void Grouding(Collision2D col, bool exitState)
    {
        // 检查状态为真
        if (exitState)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                Onground = false;
        }
        else
        {
            // 检查坠落是否触碰物体
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !Onground && col.contacts[0].normal == Vector2.up)
            {
                Onground = true;
            }
            // 检查跳跃是否触碰物体
            else if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !Onground && col.contacts[0].normal == Vector2.up)
            {

            }
            // 侧面碰墙
            else if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !Onground && (col.contacts[0].normal == Vector2.left || col.contacts[0].normal == Vector2.right))
            {

            }
        }

        animator.SetBool("Grounded", Onground);
    }
    #endregion

    #region ---- Walk function by Minqi ----
    private void Walk()
    {
        // From Idle to Walk
        if (move.x != 0 && Onground && movement_state == 0)
        {
            animator.SetInteger("AnimState", 1);
            movement_state = 1;
        }
        // From Walk to Idle
        else if (move.x == 0 && Onground && movement_state == 1)
        {
            animator.SetInteger("AnimState", 0);
            movement_state = 0;
        }
        // Cut-off if in mid-air
        if (!Onground) { movement_state = 0; }
    }
    #endregion

    #region ---- Run function by Minqi ----
    public float runSpeedMul = 1.5f;
    private void Run()
    {
        // From Walk to Run
        if (movement_state == 1)
        {
            if (Input.GetKey(KeyCode.V))
            {
                animator.SetInteger("AnimState", 2);
                movement_state = 2;
            }
        }
        // From Run to Walk
        else if (movement_state == 2)
        {
            if (!Input.GetKey(KeyCode.V))
            {
                animator.SetInteger("AnimState", 1);
                movement_state = 1;
            }
        }
        // Cut-off if in mid-air
        if (!Onground) { movement_state = 0; }
    }
    #endregion

    #region ---- Jump function by Minqi ----

    public float jumpForces = 10.0f;
    public float jumpDuration = 0.2f;

    public bool canJump = true;
    public bool isJumping = false;
    DateTime? jumpEndTime = null;
    private void Jump()
    {
        // Attach to Animator
        animator.SetFloat("AirSpeedY", rigi.velocity.y);

        // Jump if can jump
        if (canJump)
        {
            // Jump
            if(Input.GetKey(KeyCode.Space)) 
            {
                // Start jumping
                canJump = false;
                isJumping = true;

                // Set end time
                jumpEndTime = DateTime.Now.AddSeconds(jumpDuration);

                // Add jump force
                rigi.AddForce(new Vector2(0, jumpForces), ForceMode2D.Force);

                // Set jump trigger
                animator.SetTrigger("Jump");
            }
        }

        // Continue jump if jumping
        if (isJumping)
        {
            // Holding Jump
            if (Input.GetKey(KeyCode.Space))
            {
                // If still can go up
                if(DateTime.Now < jumpEndTime)
                {
                    // Add jump force
                    rigi.AddForce(new Vector2(0, jumpForces), ForceMode2D.Force);
                }
                // If jump should end
                else
                {
                    canJump = false;
                    isJumping = false;
                    jumpEndTime = null;
                }
            }
        }

        // Reset can jump if player on ground
        if (Onground)
        {
            canJump = true;
        }
    }
    #endregion
}
