using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //����InputSystem�������

public class PlayerController : MonoBehaviour //MonoBehaviour ����һ�����࣬����Unity �ű���Ĭ�������Ը��ࡣ ������Unity ����Ŀ���ڴ���һ��C# �ű�ʱ�������Զ��̳�MonoBehaviour����Ϊ���ṩģ��ű���
{
    public PlayerInputControl inputControl; // ./settings/PlayerInputControl, 
    public Vector2 inputDirection;  //./settings/PlayerInputControl��Player��Actionֵ x, y  vector 2D
    
    private PhysicsCheck physicsCheck; //����PhysicsCheck
    private Rigidbody2D rb;
    private CapsuleCollider2D coll; //����CapsuleCollider2D

    [Header("�����������: ")]
    public float jumpForce;
    public float speed;
    private float runSpeed => speed * 2f;
    private float walkSpeed;
    public bool isCrouch;
    private Vector2 originalOffset; //ԭʼ���Ҵ�С
    private Vector2 originalSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       
        inputControl = new PlayerInputControl(); //����Unity��input system
        inputControl.Player.Jump.started += Jump; //Jump��˲��ִ�еĶ���, ����ִ�к���ʱʹ��started, ʹ��+=����¼�����Jump
        
        physicsCheck = GetComponent<PhysicsCheck>(); //��ȡ����PhysicsCheck��public����
        coll = GetComponent<CapsuleCollider2D>();

        originalOffset = coll.offset;
        originalSize = coll.size;
        
        #region ��·���ܲ��л�
        walkSpeed = speed;

        inputControl.Player.RunButton.performed += ctx =>
        {
            if (physicsCheck.isGround) {speed = runSpeed;}
        };

        inputControl.Player.RunButton.canceled += ctx =>
        {
            if (physicsCheck.isGround) {speed = walkSpeed;};
        };
        #endregion
    }

    private void OnEnable()
    {
        inputControl.Enable();
       
    }

    private void OnDisable()
    {
        inputControl.Disable();
    
    }

    private void Update() //Update()��������˵����ÿˢ��һ֡��ʱ�򣬸���Щʲô --- ���ں��� Update runs once per frame.
    {
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>(); //��ȡ��������
    
    }

    private void FixedUpdate() //���ں���, FixedUpdate can run once, zero, or several times per frame, 
    {
        Move();
    
    }

    //private void OnTriggerStay2D(Collider2D other) { //��ײʱһֱִ��


        //Debug.log(other name);
    //}


    private void Move()
    {
        //�����ƶ�
        if (isCrouch == false) {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //�����ٶ�, vector��x, y����
        }

        int faceDir = (int)transform.localScale.x; //����transform��� x��ֵ, ê��λ�þ���localScale�ķ�ת, ԭ��

        if (inputDirection.x > 0) {
            faceDir = 1; //����������ߴ�, 1����2
        
        }

        if (inputDirection.x < 0) {
            faceDir = -1;
        
        }

        //���﷭ת Scale = 1�����沿������, Scale = -1�����沿������
        transform.localScale = new Vector3(faceDir, 1, 1); //����transform Scale���

        //�����¶�
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;

        if (isCrouch)
        {
            //�޸���ײ���С
            coll.offset = new Vector2(-0.07081f, 0.6055f);
            coll.size = new Vector2(0.850f, 1.21109f);

        }
        else {
            //��ԭ��ײ��
            coll.size = originalSize;
            coll.offset = originalOffset;
        }
    }

    private void Jump(InputAction.CallbackContext obj) //callbackContext�����û������¼�
    {
        if (physicsCheck.isGround) { 
           rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //�����ֲ�, �ű�APIrigidbody2d, Rigidbody2D AddForce()
        }
    }
}
