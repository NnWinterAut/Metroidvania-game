using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //调用InputSystem相关内容
using Jiahao;
using System.Text;

namespace Jiahao
{
    public class PlayerController : MonoBehaviour //MonoBehaviour 类是一个基类，所有Unity 脚本都默认派生自该类。 当您从Unity 的项目窗口创建一个C# 脚本时，它会自动继承MonoBehaviour，并为您提供模板脚本。
    {
        public PlayerInputControl inputControl; // 调用./settings/PlayerInputControl, 
        public Vector2 inputDirection;  // 调用./settings/PlayerInputControl中Player的Action值 x, y  vector 2D

        private PhysicsCheck physicsCheck; //调用PhysicsCheck
        private Rigidbody2D rb;
        private CapsuleCollider2D coll; //调用CapsuleCollider2D
        private PlayerAnimation playerAnimation;

        [Header("人物基本参数: ")]
        public float jumpForce;
        public float wallJumpForce;
        public float speed;
        private float runSpeed => speed * 1.7f;
        private float walkSpeed;
        public bool isCrouch;
        private Vector2 originalOffset; //原始胶囊大小
        private Vector2 originalSize;
        public float hurtForce;

        [Header("人物状态: ")]
        public bool isHurt;
        public bool isDead;
        public bool isAttack;
        public bool wallJump;

        [Header("物理材质")]
        public PhysicsMaterial2D normal;
        public PhysicsMaterial2D wall;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            inputControl = new PlayerInputControl(); //调用Unity的input system
            inputControl.Player.Jump.started += Jump; //Jump是瞬间执行的动作, 单次执行函数时使用started, 使用+=添加事件函数Jump

            physicsCheck = GetComponent<PhysicsCheck>(); //获取所有PhysicsCheck的public变量
            coll = GetComponent<CapsuleCollider2D>();
            playerAnimation = GetComponent<PlayerAnimation>();

            originalOffset = coll.offset;
            originalSize = coll.size;

            #region 走路和跑步切换
            walkSpeed = speed;

            inputControl.Player.RunButton.performed += ctx =>
            {
                if (physicsCheck.isGround) { speed = runSpeed; }
            };

            inputControl.Player.RunButton.canceled += ctx =>
            {
                if (physicsCheck.isGround) { speed = walkSpeed; };
            };
            #endregion

            #region 攻击
            inputControl.Player.Attack.started += PlayerAttack;
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

        private void Update() //Update()函数就是说，在每刷新一帧的时候，该做些什么 --- 周期函数 Update runs once per frame.
        {
            inputDirection = inputControl.Player.Move.ReadValue<Vector2>(); //读取键盘输入

            CheckState();

        }

        private void FixedUpdate() //周期函数, FixedUpdate can run once, zero, or several times per frame, 
        {
            if (!isHurt && !isAttack)
            {
                Move();
            }
        }

        private void Move()
        {
            //人物移动
            if (isCrouch == false && !wallJump)
            {
                rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //人物速度, vector的x, y向量
            }

            int faceDir = (int)transform.localScale.x; //调用transform组件 x的值, 锚点位置决定localScale的翻转, 原地

            if (inputDirection.x > 0)
            {
                faceDir = 1; //决定于引物尺寸, 1还是2

            }

            if (inputDirection.x < 0)
            {
                faceDir = -1;

            }

            //人物翻转 Scale = 1人物面部朝向右, Scale = -1人物面部朝向左
            transform.localScale = new Vector3(faceDir, 1, 1); //调用transform Scale组件

            //人物下蹲
            isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;

            if (isCrouch)
            {
                //修改碰撞体大小
                coll.offset = new Vector2(-0.07081f, 0.6055f);
                coll.size = new Vector2(0.850f, 1.21109f);

            }
            else
            {
                //还原碰撞体
                coll.size = originalSize;
                coll.offset = originalOffset;
            }
        }

        private void Jump(InputAction.CallbackContext obj) //callbackContext传输用户输入事件
        {
            if (physicsCheck.isGround)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //代码手册, 脚本APIrigidbody2d, Rigidbody2D AddForce()
            }
            else if (physicsCheck.onWall) {
                //登墙跳
                wallJump = true;

                rb.AddForce(new Vector2(-inputDirection.x,2f)* wallJumpForce, ForceMode2D.Impulse);
            }
        }

        private void PlayerAttack(InputAction.CallbackContext obj)
        {
            if (physicsCheck.isGround) //人物空中无法攻击
            {
                playerAnimation.PlayAttack();
                isAttack = true;
            }
        }

        public void GetHurt(Transform attacker)
        {
            isHurt = true;
            rb.velocity = Vector2.zero;
            Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
            rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

        }

        public void PlayerDead()
        {
            isDead = true;
            inputControl.Player.Disable();

        }

        private void CheckState()
        {

            coll.sharedMaterial = physicsCheck.isGround ? normal : wall;

            if (physicsCheck.onWall) //如果在墙面
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f); //改变y轴速度
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            if (wallJump && rb.velocity.y < 0f) { //落地

                wallJump = false;
            }
        }

    }
}