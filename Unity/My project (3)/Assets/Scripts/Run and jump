using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{

    private Rigidbody2D rigi;
    private Animator animator;

    float moveSpeed = 2.0f;
 
    float jumpForces = 10.0f;
    bool facingRight = true;

    bool Onground;

    int moveChangeAni;

    bool canJump = true;
    bool isJumping = false;
    const float JUMPDURATION = 0.2f;
    private float jumpDuration = JUMPDURATION;

    // Start is called before the first frame update
    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Direction();
        Jump();

        Run();
    }

    float moveX;
    float moveY;
    private void Movement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        rigi.velocity = new Vector2(moveX * moveSpeed * (isRunning ? runSpeedMul : 1), rigi.velocity.y);

        if (moveX > 0)
        {
            moveChangeAni = 1;
        }
        else if (moveX < 0)
        {
            moveChangeAni = -1;
        }
        else
        {
            moveChangeAni = 0;
        }

        animator.SetInteger("movement", moveChangeAni);
    }

    private void Direction()
    {
        if (moveX > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveX < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight; //Inverse.bool
        transform.Rotate(0f, 180f, 0f);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && (canJump || isJumping))
        {
            if (!isJumping) { rigi.velocity.Set(rigi.velocity.x, 0); }

            canJump = false;
            isJumping = true;

            rigi.AddForce(new Vector2(0, jumpForces), ForceMode2D.Force);

            animator.SetTrigger("jump");

            jumpDuration -= Time.deltaTime;
            if (jumpDuration <= 0) { isJumping = false; }
        }
        else
        {
            isJumping = false;
        }
    }

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

    public float runSpeedMul = 1.5f;
    bool isRunning = false;
    private void Run()
    {
        if(Input.GetKey(KeyCode.V) && Onground)
        {
            animator.SetInteger("AnimState", 1);
            isRunning = true;
        }
        else
        {
            animator.SetInteger("AnimState", 0);
            isRunning = false;
        }
    }
}
