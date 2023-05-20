using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class PlayerAnimation : MonoBehaviour //�������ﶯ���Ľű�
    {
        private Animator anim;
        private Rigidbody2D rb;
        private PhysicsCheck physicsCheck;
        private PlayerController playerController;


        private void Awake()
        {
            anim = GetComponent<Animator>(); //��ȡAnimatorʹ��Ȩ
            rb = GetComponent<Rigidbody2D>(); //��ȡ���������x, y����ٶ�
            physicsCheck = GetComponent<PhysicsCheck>();
            playerController = GetComponent<PlayerController>();

        }

        public void Update()
        {
            SetAnimation(); //ʵʱ�Ķ����л�, ÿһ֡��Ҫִ��
        }

        public void SetAnimation() //ִ��ÿһ֡�Ķ����л�
        {
            anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x)); //�����ƶ�����
            anim.SetFloat("velocityY", rb.velocity.y);  //������Ծ����
            anim.SetBool("isGround", physicsCheck.isGround); //������Χ�������
            anim.SetBool("isCrouch", playerController.isCrouch); //�����¶׶���
            anim.SetBool("isDead", playerController.isDead); //��������
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