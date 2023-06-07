using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //����InputSystem�������
using System;



    public class PlayerController : MonoBehaviour //MonoBehaviour ����һ�����࣬����Unity �ű���Ĭ�������Ը��ࡣ ������Unity ����Ŀ���ڴ���һ��C# �ű�ʱ�������Զ��̳�MonoBehaviour����Ϊ���ṩģ��ű���
    {
        [Header("����Event")]
        public SceneLoadEventSO sceneLoadEvent;
        public VoidEventSO afterSceneLoadedEvent;
        public VoidEventSO loadDataEvent;
        public VoidEventSO backToMenu;
        public PlayerInputControl inputControl; // ����./settings/PlayerInputControl, 
        public Vector2 inputDirection;  // ����./settings/PlayerInputControl��Player��Actionֵ x, y  vector 2D

        private PhysicsCheck physicsCheck; //����PhysicsCheck
        private Rigidbody2D rb;
        private CapsuleCollider2D coll; //����CapsuleCollider2D
        private PlayerAnimation playerAnimation;
        private Character character;

        [Header("�����������: ")]
        public float jumpForce;
        public float wallJumpForce;
        public float speed;
        private float runSpeed => speed * 1.7f;
        private float walkSpeed;
        public bool isCrouch;
        private Vector2 originalOffset; //ԭʼ���Ҵ�С
        private Vector2 originalSize;
        public float hurtForce;
        public float slideDistance;
        public float slideSpeed;
        public int slidePowerCost;

        [Header("����״̬: ")]
        public bool isHurt;
        public bool isDead;
        public bool isAttack;
        public bool wallJump;
        public bool isSlide;

        [Header("��������")]
        public PhysicsMaterial2D normal;
        public PhysicsMaterial2D wall;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            character = GetComponent<Character>();
            physicsCheck = GetComponent<PhysicsCheck>(); //��ȡ����PhysicsCheck��public����
            coll = GetComponent<CapsuleCollider2D>();
            playerAnimation = GetComponent<PlayerAnimation>();

            inputControl = new PlayerInputControl(); //����Unity��input system
            inputControl.Player.Jump.started += Jump; //Jump��˲��ִ�еĶ���, ����ִ�к���ʱʹ��started, ʹ��+=�����¼�����Jump

            originalOffset = coll.offset;
            originalSize = coll.size;

            inputControl.Enable();

            #region ��·���ܲ��л�
            walkSpeed = speed;

            inputControl.Player.RunButton.performed += ctx =>
            {
                if (physicsCheck.isGround)
                {
                    speed = runSpeed;
                }
                else {
                    speed = walkSpeed;
                }
            };

            inputControl.Player.RunButton.canceled += ctx =>
            {
                if (physicsCheck.isGround) 
                { 
                    speed = walkSpeed; 
                }
                else
                {
                    speed = walkSpeed;
                }
            };
            #endregion

            #region ����
            inputControl.Player.Attack.started += PlayerAttack;
            #endregion

            #region ����
            inputControl.Player.Slide.started += Slide;
            #endregion
        
        }

        private void OnEnable()
        {
            sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
            afterSceneLoadedEvent.OnEventRaised += OnafterSceneLoadedEvent;
            loadDataEvent.OnEventRaised += OnLoadDataEvent;
            backToMenu.OnEventRaised += OnLoadDataEvent;

        }

        private void OnDisable()
        {
            inputControl.Disable();
            sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
            afterSceneLoadedEvent.OnEventRaised -= OnafterSceneLoadedEvent;
            loadDataEvent.OnEventRaised -= OnLoadDataEvent;
            backToMenu.OnEventRaised -= OnLoadDataEvent;

        }
        private void OnLoadDataEvent()
        {
            isDead = false; //��ȡ��Ϸ����
        }

        private void OnafterSceneLoadedEvent()
        {
            inputControl.Player.Enable(); //�������ؽ�ֹ�ƶ�
        }

        private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
        {
            inputControl.Player.Disable(); //�ָ��ƶ�
        }

        private void Update() //Update()��������˵����ÿˢ��һ֡��ʱ�򣬸���Щʲô --- ���ں��� Update runs once per frame.
        {
            inputDirection = inputControl.Player.Move.ReadValue<Vector2>(); //��ȡ��������

            CheckState();

        }

        private void FixedUpdate() //���ں���, FixedUpdate can run once, zero, or several times per frame, 
        {
            if (!isHurt && !isAttack)
            {
                Move();
            }
        }

        private void Move()
        {
            //�����ƶ�
            if (isCrouch == false && !wallJump)
            {
                rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //�����ٶ�, vector��x, y����
            }

            int faceDir = (int)transform.localScale.x; //����transform��� x��ֵ, ê��λ�þ���localScale�ķ�ת, ԭ��

            if (inputDirection.x > 0)
            {
                faceDir = 1; //����������ߴ�, 1����2

            }

            if (inputDirection.x < 0)
            {
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
            else
            {
                //��ԭ��ײ��
                coll.size = originalSize;
                coll.offset = originalOffset;
            }
        }

        private void Jump(InputAction.CallbackContext obj) //callbackContext�����û������¼�
        {
            if (physicsCheck.isGround)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //�����ֲ�, �ű�APIrigidbody2d, Rigidbody2D AddForce()
                
                GetComponent<AudioDefination>().PlayAudioClip(); //������Ч

                //��ϻ���Я��
                isSlide = false;
                StopAllCoroutines();
            }
            else if (physicsCheck.onWall) {
                //��ǽ��
                wallJump = true;

                rb.AddForce(new Vector2(-inputDirection.x,2f)* wallJumpForce, ForceMode2D.Impulse);
                GetComponent<AudioDefination>().PlayAudioClip(); //������Ч

            }
        }

        private void PlayerAttack(InputAction.CallbackContext obj)
        {
            if (physicsCheck.isGround) //��������޷�����
            {
                playerAnimation.PlayAttack();
                isAttack = true;
            }
        }

        private void Slide(InputAction.CallbackContext obj)
        {
            if (isSlide == false && physicsCheck.isGround && character.currentPower >= slidePowerCost)
            {
                isSlide = true;

                var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);

                gameObject.layer = LayerMask.NameToLayer("Enemies"); //滑铲躲避敌人，切换Layer
                StartCoroutine(TriggerSlide(targetPos));

                GetComponent<AudioDefination>().PlayAudioClip(); //播放音乐

                character.OnSlide(slidePowerCost);//cost energy
            }

        }

        private IEnumerator TriggerSlide(Vector3 target)
        {
            do
            {
                yield return null;

                if (physicsCheck.isGround == false) { 
                    break;
                }

                //����������ײǽ, �泯����ײǽ
                if (physicsCheck.touchLeftWall && transform.localScale.x < 0f || physicsCheck.touchRightWall && transform.localScale.x > 0f)
                {
                    isSlide = false;
                    break;
                }

                rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
            
            } while (MathF.Abs(target.x - transform.position.x) > 0.1f);

            isSlide = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
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

            if (physicsCheck.onWall) //�����ǽ��
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2f); //�ı�y���ٶ�
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            if (wallJump && rb.velocity.y < 0f)
            { //���

                wallJump = false;
            }

            //����ʱҲҪ�л� Player �� Layer ���ⷢ���� Enemy ����ײ
            if (isDead || isSlide)
            {  
                gameObject.layer = LayerMask.NameToLayer("Enemies");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        
        }

    }
