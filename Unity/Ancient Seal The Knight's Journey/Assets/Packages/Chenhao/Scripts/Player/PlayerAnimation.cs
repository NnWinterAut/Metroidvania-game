using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chenhao
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;

        //GetComponent from Rigidbody 2D
        private Rigidbody2D rb;

        //GetComponent from PhysicsCheck
        private PhysicsCheck physicsCheck;

        //GetComponent from PlayerController
        private PlayerController playerController;


        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            physicsCheck = GetComponent<PhysicsCheck>();
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            SetAnimation();
        }

        public void SetAnimation()
        {
            anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("velocityY", rb.velocity.y);
            anim.SetBool("isGround", physicsCheck.isGround);
            anim.SetBool("isDead", playerController.isDead);
            anim.SetBool("isAttack", playerController.isAttack);
        }

        public void PlayHurt()
        {
            anim.SetTrigger("hurt");

        }

        public void PlayAttack()
        {
            anim.SetTrigger("attack");
        }
    }
}