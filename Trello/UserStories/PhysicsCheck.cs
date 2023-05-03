using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    //������

    public bool isGround; //��������Ƿ��ڵ���
    public float checkRaduis;
    public Vector2 bottomOffset; //�ŵ�λ�Ʋ�ֵ
    public LayerMask groundLayer; //ͼ��ѡ��

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);

    }

    private void onDrwaGizmosSelected()//��������ŵ�λ�Ʋ�ֵ
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    
    }


}
