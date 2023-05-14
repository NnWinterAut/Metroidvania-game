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
        //当攻击时候不可移动
        if (!attackModel)
        {
            move();
        }
        ///当敌人超出边界斌且玩家不在范围里面并且敌人没有攻击
        if (!InsideofLimits()&&!Range && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            ObjectSelectorTargetInfo();
        }
        if (Range)
        {
            //发出射线并存储
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
    //玩家是否进入触发区
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //存储游戏对象到变量
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

    //敌人是否在边界
    private bool InsideofLimits()
    {
        //检查敌人的位置是否大于左边界或者小于有边界
        return transform.position.x > leftPoint.position.x&&transform.position.x< rigitPoint.position.x;
    }

    //让敌人移动到目标
    private void ObjectSelectorTargetInfo()
    {
        //计算敌人到左边界与有边界的距离
        float distanceToLeft = Vector2.Distance(transform.position, leftPoint.position);
        float distanceToRight = Vector2.Distance(transform.position, rigitPoint.position);

        //如果左边界距离大于有边界距离，设置到左限制
        if(distanceToLeft > distanceToRight)
        {
            target = leftPoint;
        }
        else//右限制
        {
            target = rigitPoint;
        }
        Flip();
    }

    //翻转敌人
    private void Flip()
    {
        //存储翻转角度
        Vector3 rotation = transform.localEulerAngles;
        //检测敌人位置是否大于目标位置
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        //初始角度
        transform.eulerAngles = rotation;
    }
}


