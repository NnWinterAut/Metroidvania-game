using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Unity variables
    private Rigidbody2D rigi;
    public SpriteRenderer sr;
    public Transform leftPoint, rigitPoint;
    private Animator animator;
    public Transform Cast;
    public LayerMask CastMask;

    private RaycastHit2D hit;
    private Transform target;
    #endregion

    #region game variables
    public float moveSpeed;
    public float moveTime, waitTime;
    private float moveCount, waitCount;
    public float CastLength;
    public float attackDistance;
    public float timer; //cooldown of attacks
    private bool attackModel;
    private float distance; //Stroe the distance between enemy and player
    private bool Range; //check if player in range
    private bool cooling;//check if Enemy is cooling after attack
    private float intTimer;
    #endregion
    void Start()
    {
        rigi=GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        intTimer = timer;
        ObjectSelectorTargetInfo();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        RangeToAttack();
        Attack();
        StopAttack();
    }
    private void move()
    {
            animator.SetInteger("AnimState", 2);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
    }

    private void Attack()
    {
        //Reset Timer whem player enter attack range
        timer = intTimer;
        //Check if enemy can still attack or not
        attackModel = true;

        animator.SetBool("CanWalk", false);
        animator.SetBool("Attack", true);
    }
    private void StopAttack()
    {
        cooling = false;
        attackModel = false;
        animator.SetBool("Attack", false);
    }

    private void RangeToAttack()
    {
        //������ʱ�򲻿��ƶ�
        if (!attackModel)
        {
            move();
        }
        ///�����˳����߽������Ҳ��ڷ�Χ���沢�ҵ���û�й���
        if (!InsideofLimits()&&!Range && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            ObjectSelectorTargetInfo();
        }
        if (Range)
        {
            //�������߲��洢
            hit = Physics2D.Raycast(Cast.position, transform.right, CastLength, CastMask);
            CastDebugger();
        }
        //when player detected
        if(hit.collider != null)
        {
            EnemyLogic();
        }
        else if(hit.collider == null)
        {
            Range = false;
        }
        if (Range == false)
        {
            StopAttack();
        }
    }
    //����Ƿ���봥����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�洢��Ϸ���󵽱���
            target = collision.transform;
            Range = true;
            Flip();
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false){
            Attack();
        }
        if (cooling)
        {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }
    private void CastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(Cast.position, transform.right * CastLength, Color.red);
        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(Cast.position, transform.right* CastLength, Color.green);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackModel)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    //�����Ƿ��ڱ߽�
    private bool InsideofLimits()
    {
        //�����˵�λ���Ƿ������߽����С���б߽�
        return transform.position.x > leftPoint.position.x&&transform.position.x< rigitPoint.position.x;
    }

    //�õ����ƶ���Ŀ��
    private void ObjectSelectorTargetInfo()
    {
        //������˵���߽����б߽�ľ���
        float distanceToLeft = Vector2.Distance(transform.position, leftPoint.position);
        float distanceToRight = Vector2.Distance(transform.position, rigitPoint.position);

        //�����߽��������б߽���룬���õ�������
        if(distanceToLeft > distanceToRight)
        {
            target = leftPoint;
        }
        else//������
        {
            target = rigitPoint;
        }
        Flip();
    }

    //��ת����
    private void Flip()
    {
        //�洢��ת�Ƕ�
        Vector3 rotation = transform.localEulerAngles;
        //������λ���Ƿ����Ŀ��λ��
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        //��ʼ�Ƕ�
        transform.eulerAngles = rotation;
    }
}


