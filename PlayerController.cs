using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour //MonoBehaviour 类是一个基类，所有Unity 脚本都默认派生自该类。 当您从Unity 的项目窗口创建一个C# 脚本时，它会自动继承MonoBehaviour，并为您提供模板脚本。
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

    private void Update() //Update()函数就是说，在每刷新一帧的时候，该做些什么 --- 周期函数
    {
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
    
    }

    private void FixedUpdate() //周期函数
    {
        Move();
    
    }

    private void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //人物速度, vector的x, y向量

        int faceDir = (int)transform.localScale.x; //调用transform组件 x的值, 锚点位置决定localScale的翻转, 原地

        if (inputDirection.x > 0) {
            faceDir = 1; //决定于引物尺寸, 1还是2
        
        }

        if (inputDirection.x < 0) {
            faceDir = -1;
        
        }

        //人物翻转 Scale = 1人物面部朝向右, Scale = -1人物面部朝向左
        transform.localScale = new Vector3(faceDir, 1, 1); //调用transform组件
    }
}
