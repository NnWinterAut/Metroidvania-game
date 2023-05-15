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
    private void FixedUpdate()
    {
        Jumping();
        StaminaRegen();
    }

    #region ---- Animator by Penelope & Winter ----
    private Rigidbody2D rigi;
    private Animator animator;
    private Player player;

    // Start is called before the first frame update
    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    #endregion

    #region ---- Movement by Penelope & Winter ----
    Vector2 move = new (0, 0);
    bool Onground;
    int movement_state = 0;
    bool facingRight = true;
    private void Movement()
    {
        // Get the input of the player
        move = new Vector2(Input.GetAxis("Horizontal"), 0);

        // Move the character
        rigi.velocity = new Vector2(move.x * player.speed * (movement_state == 2 ? runSpeedMul : 1), rigi.velocity.y);

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

    public float runSpeedMul = 1.35f;
    public float stamina = 100.0f;
    public float staminaMax = 100.0f;
    public float staminaMin = 20.0f;
    public float staminaRegen = 10.0f;
    public float staminaDec = 50.0f;
    private void Run()
    {
        // From Walk to Run
        if (movement_state == 1 && stamina > staminaMin)
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
            if (!Input.GetKey(KeyCode.V) || stamina <= 0f)
            {
                animator.SetInteger("AnimState", 1);
                movement_state = 1;
            }
            else 
            {
                stamina -= staminaDec * Time.deltaTime;
            }
        }
        // Cut-off if in mid-air
        if (!Onground) { movement_state = 0; }
    }
    private void StaminaRegen()
    {
        // Regen Stamina when not running
        if(movement_state != 2 && stamina < staminaMax)
        {
            var regenTo = stamina + (staminaRegen * Time.deltaTime);
            stamina = regenTo <= staminaMax ? regenTo : staminaMax;
        }
    }
    #endregion

    #region ---- Jump function by Minqi ----

    public float jumpForces = 0.6f;
    public float jumpDuration = 0.2f;

    public bool canJump = true;
    public bool isJumping = false;
    private float jumpTimer = 0.0f;
    private void Jump()
    {
        // Attach to Animator
        animator.SetFloat("AirSpeedY", rigi.velocity.y);

        // Jump if can jump
        if (canJump)
        {
            // Jump
            if(Input.GetButtonDown("Jump")) 
            {
                // Start jumping
                canJump = false;
                isJumping = true;

                // Set end time
                jumpTimer = 0.0f;

                // Set jump trigger
                animator.SetTrigger("Jump");
            }
        }

        // Reset can jump if player on ground
        if (Onground)
        {
            canJump = true;
        }
    }
    /// <summary>
    /// This must be called in FixedUpdate
    /// </summary>
    private void Jumping()
    {
        // Continue jump if jumping
        if (isJumping)
        {
            // Holding Jump
            if (Input.GetButton("Jump"))
            {
                // If still can go up
                if (jumpTimer < jumpDuration)
                {
                    // Add jump force
                    rigi.AddForce(new Vector2(0, jumpForces), ForceMode2D.Impulse);
                    jumpTimer += Time.deltaTime;
                }
                // If jump should end
                else
                {
                    canJump = false;
                    isJumping = false;
                }
            }
        }
    }
    #endregion
}
