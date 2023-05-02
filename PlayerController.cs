using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour //MonoBehaviour ����һ�����࣬����Unity �ű���Ĭ�������Ը��ࡣ ������Unity ����Ŀ���ڴ���һ��C# �ű�ʱ�������Զ��̳�MonoBehaviour����Ϊ���ṩģ��ű���
{
    public PlayerInputControl inputControl; // ./settings/PlayerInputControl
    public Vector2 inputDirection;  //x, y vector 2D
    public Rigidbody2D rb; // unity.Rigidbody2D
    //private Rigidbody2D rb;
    public float speed;

    private void Awake()
    {
        inputControl = new PlayerInputControl();

        //rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        inputControl.Enable();
       
    }

    private void OnDisable()
    {
        inputControl.Disable();
    
    }

    private void Update() //Update()��������˵����ÿˢ��һ֡��ʱ�򣬸���Щʲô --- ���ں���
    {
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
    
    }

    private void FixedUpdate() //���ں���
    {
        Move();
    
    }

    private void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //�����ٶ�, vector��x, y����

        int faceDir = (int)transform.localScale.x; //����transform��� x��ֵ, ê��λ�þ���localScale�ķ�ת, ԭ��

        if (inputDirection.x > 0) {
            faceDir = 1; //����������ߴ�, 1����2
        
        }

        if (inputDirection.x < 0) {
            faceDir = -1;
        
        }

        //���﷭ת Scale = 1�����沿������, Scale = -1�����沿������
        transform.localScale = new Vector3(faceDir, 1, 1); //����transform���
    }
}
