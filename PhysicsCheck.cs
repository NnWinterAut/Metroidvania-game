using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    //物理检测

    public bool isGround; //检测人物是否在地面
    public float checkRaduis;
    public Vector2 bottomOffset; //脚底位移差值
    public LayerMask groundLayer; //图层选择

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);

    }

    private void onDrwaGizmosSelected()//绘制人物脚底位移差值
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    
    }


}
