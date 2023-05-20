using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void PlayHurt() 
    {
        anim.SetTrigger("hurt");
       
    }
}
