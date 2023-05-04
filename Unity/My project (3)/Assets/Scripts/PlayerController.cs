using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigi;
    private Animator animator;

    float moveSpeed = 2.0f;
    float jumpForces = 10.0f;
    bool facingRight = true;

    bool Onground;

    int moveChangeAni;
    int run = 0;

    public float moveX;
    float moveY;

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

        RunMux();
        Run();
    }
    private void Movement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        rigi.velocity = new Vector2(moveX * moveSpeed * (run > 0 ? 5f : 1), rigi.velocity.y);

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
                ResetJump();
                JumpCancle();
            }
            // 检查跳跃是否触碰物体
            else if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !Onground && col.contacts[0].normal == Vector2.up)
            {
                // Double Jump
                ResetJump();
                JumpCancle();
            }
            // 侧面碰墙
            else if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !Onground && (col.contacts[0].normal == Vector2.left || col.contacts[0].normal == Vector2.right))
            {
            }
        }

        animator.SetBool("onground", Onground);
    }

    private void JumpCancle()
    {
        animator.ResetTrigger("jump");
    }
    private void ResetJump()
    {
        canJump = true;
        jumpDuration = JUMPDURATION;
    }

    const float RUN_DOUBLE_INTERVAL = 0.5f;
    private float runDoubleInterval = RUN_DOUBLE_INTERVAL;
    public int run_state = 0;
    public float run_x = 0;

    private void Run()
    {
        if (!Onground) { run_state = 0; }
        // 速度稳定
        if (run_mux == 0 && run_state == 0)
        {
            run_state = 1;
            runDoubleInterval = RUN_DOUBLE_INTERVAL;
            return;
        }
        // 加速
        if (run_mux > 0 && run_state == 1)
        {
            run_state = 2; return;
        }
        // 减速

        if (run_state >= 2 && run != 1)
        {
            runDoubleInterval -= Time.deltaTime;
            if (runDoubleInterval <= 0) { run_state = 0; runDoubleInterval = RUN_DOUBLE_INTERVAL; }
        }


        if (run_mux < 0 && run_state == 2)
        {
            run_state = 3; return;
        }

        if (run_mux > 0 && run_state == 3)
        {
            run_state = 4;
        }

        if (run_state == 4)
        {
            if (run_mux >= 0) { run = 1; }
            else
            {
                run = 0;
                run_mux = 0;
                run_state = 0; runDoubleInterval = RUN_DOUBLE_INTERVAL;
            }
        }

        animator.SetInteger("run", run);
    }

    public int run_mux = 0;
    private void RunMux()
    {
        if (run_x > Math.Abs(moveX) && run_mux < 1) { run_mux = -1; run_x = Math.Abs(moveX); return; }
        if (run_x < Math.Abs(moveX) && run_mux > -1) { run_mux = 1; run_x = Math.Abs(moveX); return; }
        run_mux = 0;
    }
}
