using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]

    public class Enemy : MonoBehaviour
    {
        //����
        [HideInInspector] public Rigidbody2D rb; 
        [HideInInspector] public PhysicsCheck physicsCheck;
        [HideInInspector] public Animator anim;

        [Header("��������: ")]
        public float normalSpeed;//�����ٶ�
        public float chaseSpeed;
        public float currentSpeed;
        public Vector3 faceDir;  //����������
        public Transform attacker; //������Դ
        public float hurtForce; //���˺��˵���
        public Vector3 spwanPoint; //���������

        [Header("��ʱ��")] //����Ѳ��ײǽ, ͣ��ʱ��
        public bool wait;
        public float waitTime;
        public float waitTimeCounter;
        public float lostTime;
        public float lostTimeCounter; //�˳�״̬ʱ��

        [Header("״̬")]
        public bool isHurt;
        public bool isDead;
        private BaseState currentState;
        protected BaseState patrolState;
        protected BaseState chaseState;
        protected BaseState skillState;

        [Header("���")]
        public Vector2 centerOffset;
        public Vector2 checkSize;
        public float checkDistance;
        public LayerMask attackLayer;

        protected virtual void Awake() //Ϊ�������û������, ���Ը�Ϊvirtualд��
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            physicsCheck = GetComponent<PhysicsCheck>();

            currentSpeed = normalSpeed; //�����ٶ�
            spwanPoint = transform.position;
        }

        private void OnEnable() //����״̬ unity״̬��
        {
            currentState = patrolState;
            currentState.OnEnter(this);

        }

        public void Update() //��ʼ��Ⱦ�����µ�һ֡
        {
            faceDir = new Vector3(-transform.localScale.x, 0, 0); //�޸�transform���ı�����泯����

            currentState.LogicUpdate();
            TimeCounter();
        }

        private void FixedUpdate() //��ʼ������������߼�, ÿ���̶���ʱ����������
        {
            if (!isHurt && !isDead && !wait)
            {
                Move();
            }
            currentState.PhysicsUpdate();
        }

        private void OnDisable() //�˳�״̬
        {
            currentState.OnExit();

        }
        public virtual void Move() //����Boar�ɽ��б༭
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("snailPremove") && !anim.GetCurrentAnimatorStateInfo(0).IsName("snailRecover"))
            {  //���û��premove��һֱ�ƶ�
                rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
            }

        }

        public void TimeCounter() //����Ѳ��ײǽ, ͣ��ʱ��, ��״̬���л�ʱ���ʱ
        {
            // ��ʱ��
            if (wait)
            {

                waitTimeCounter -= Time.deltaTime;

                if (waitTimeCounter <= 0)
                {

                    wait = false;
                    waitTimeCounter = waitTime;
                    transform.localScale = new Vector3(faceDir.x, 1, 1); //����ת��

                }
            }

            if (!FoundPlayer() && lostTimeCounter > 0) //��ʧ���
            {

                lostTimeCounter -= Time.deltaTime;

            }

        }

        public void OnDie() //��������
        {
            gameObject.layer = 2; //���������л�ͼ��
            anim.SetBool("dead", true); //��������
            isDead = true;
        }

        public void DestroyAfterAnimation() //ɾ������
        {
            Destroy(this.gameObject);

        }

        public void OnTakeDamage(Transform attackTrans)
        {

            attacker = attackTrans;
            //����ת��, ���򹥻���
            if (attackTrans.position.x - transform.position.x > 0)
            {

                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (attackTrans.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            //����
            isHurt = true;
            anim.SetTrigger("hurt"); //���˶���

            Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
            rb.velocity = new Vector2(0, rb.velocity.y);

            StartCoroutine(OnHurt(dir)); //��ʼЭ��
        }

        private IEnumerator OnHurt(Vector2 dir) //Unity ������д��, ������Դ:https://docs.unity3d.com/cn/2021.2/Manual/Coroutines.html
        {
            //�������˺��ұ����˺�, �ȴ�, ���ָ���δ�ܵ��˺���״̬
            rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);

            isHurt = false;
        }

        public virtual Vector3 GetNewPoint()
        { //��������ƶ���, �ᱻ��д

            return transform.position;
        }

        public virtual bool FoundPlayer() //���е��˸�д�ò���
        {
            return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);

        }

        public void SwitchState(NPCState state) //״̬ת��
        {
            var newState = state switch
            {
                NPCState.Patrol => patrolState,
                NPCState.Chase => chaseState,
                NPCState.Skill => skillState,
                _ => null
            };

            currentState.OnExit(); //�˳�״̬
            currentState = newState; //���
            currentState.OnEnter(this); //������״̬

        }

        public virtual void OnDrawGizmosSelected() //���������ҷ�Χ
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance * -transform.localScale.x, 0), 0.2f);

        }
    }
}