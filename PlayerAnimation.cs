using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour //控制人物动画的脚本
{
    private Animator anim; 
    private Rigidbody2D rb;

    private void Awake() 
    {
        anim = GetComponent<Animator>(); //获取Animator使用权
        rb = GetComponent<Rigidbody2D>();
    
    }

    public void SetAnimation() //动画切换
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
    
    }

    public void Update() 
    {

        SetAnimation(); //实时的动画切换, 每一帧都要执行
    
    }

}
