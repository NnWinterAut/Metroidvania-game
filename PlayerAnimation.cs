using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour //�������ﶯ���Ľű�
{
    private Animator anim; 
    private Rigidbody2D rb;

    private void Awake() 
    {
        anim = GetComponent<Animator>(); //��ȡAnimatorʹ��Ȩ
        rb = GetComponent<Rigidbody2D>();
    
    }

    public void SetAnimation() //�����л�
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
    
    }

    public void Update() 
    {

        SetAnimation(); //ʵʱ�Ķ����л�, ÿһ֡��Ҫִ��
    
    }

}
