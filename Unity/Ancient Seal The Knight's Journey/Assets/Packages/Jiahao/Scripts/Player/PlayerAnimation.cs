using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class PlayerAnimation : MonoBehaviour //控制人物动画的脚本
    {
        private Animator anim;
        private Rigidbody2D rb;
        private PhysicsCheck physicsCheck;
        private PlayerController playerController;


        private void Awake()
        {
            anim = GetComponent<Animator>(); //获取Animator使用权
            rb = GetComponent<Rigidbody2D>(); //获取刚体组件的x, y轴的速度
            physicsCheck = GetComponent<PhysicsCheck>();
            playerController = GetComponent<PlayerController>();

        }

        public void Update()
        {
            SetAnimation(); //实时的动画切换, 每一帧都要执行
        }

        public void SetAnimation() //执行每一帧的动画切换
        {
            anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x)); //人物移动动画
            anim.SetFloat("velocityY", rb.velocity.y);  //人物跳跃动画
            anim.SetBool("isGround", physicsCheck.isGround); //人物周围环境检测
            anim.SetBool("isCrouch", playerController.isCrouch); //人物下蹲动作
            anim.SetBool("isDead", playerController.isDead); //人物死亡
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