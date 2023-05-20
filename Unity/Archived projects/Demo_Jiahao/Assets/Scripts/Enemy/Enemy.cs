using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator),typeof(PhysicsCheck))]

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public PhysicsCheck physicsCheck;
    public Animator anim;

    [Header("基本参数: ")]
    public float normalSpeed;//敌人速度
    public float chaseSpeed;
    public float currentSpeed;  
    public Vector3 faceDir;  //敌人面向方向
    public Transform attacker; //攻击来源
    public float hurtForce; //受伤后退的力
    public Vector3 spwanPoint; //随机出生点

    [Header("计时器")] //敌人巡逻撞墙, 停滞时间
    public bool wait;
    public float waitTime;   
    public float waitTimeCounter;
    public float lostTime;
    public float lostTimeCounter; //退出状态时间

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState skillState;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    protected virtual void Awake() //为了子类获得基础组件, 尝试改为virtual写法
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = normalSpeed; //敌人速度
        waitTimeCounter = waitTime;
        spwanPoint = transform.position;
    }

    private void OnEnable() //激活状态 unity状态机
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    
    }

    public void Update() //开始渲染敌人新的一帧
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0); //修改transform来改变敌人面朝方向
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate() //开始处理敌人物理逻辑, 每个固定的时间间隔被调用
    {
        if (!isHurt && !isDead && !wait)
        {
            Move();
        }
        currentState.PhysicsUpdate();
    }



    private void OnDisable() //退出状态
    {
        currentState.OnExit();
       
    }

    public void TimeCounter() //敌人巡逻撞墙, 停滞时间, 和状态机切换时间计时
    {
        if (wait) {

            waitTimeCounter -= Time.deltaTime;

            if (waitTimeCounter <= 0) {

                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1); //敌人转身

            }
        }

        if (!FoundPlayer() && lostTimeCounter > 0) //丢失玩家
        {

            lostTimeCounter -= Time.deltaTime;

        }
        else if (FoundPlayer())    //在发现玩家的时候重置丢失时间
        {
            lostTimeCounter = lostTime;
        }

    }

    public virtual void Move() //子类Boar可进行编辑
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("snailPremove") && !anim.GetCurrentAnimatorStateInfo(0).IsName("snailRecover")) {  //如果没有premove则一直移动
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }

    }

    public void OnDie() //敌人死亡
    {
        gameObject.layer = 2; //敌人死亡切换图层
        anim.SetBool("dead", true); //死亡动画
        isDead = true;
    }

    public void DestroyAfterAnimation() //删除动画
    {
        Destroy(this.gameObject);
    
    }

    public void OnTakeDamage(Transform attackTrans) {

        attacker = attackTrans;
        //敌人转身, 面向攻击者
        if (attackTrans.position.x - transform.position.x > 0) {

            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (attackTrans.position.x - transform.position.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //受伤
        isHurt = true;
        anim.SetTrigger("hurt"); //受伤动画

        rb.velocity = new Vector2(0, rb.velocity.y);
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        
        StartCoroutine(OnHurt(dir)); //开始协成
    }

    private IEnumerator OnHurt(Vector2 dir) //Unity 迭代器写法, 代码来源:https://docs.unity3d.com/cn/2021.2/Manual/Coroutines.html
    {   
        //敌人受伤后并且被击退后, 等待, 并恢复到未受到伤害的状态
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        
        isHurt = false;
    }

    public virtual Vector3 GetNewPoint() { //敌人随机移动点, 会被复写

        return transform.position;
    }

    public virtual bool FoundPlayer() //飞行敌人复写该部分
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);

    }

    public void SwitchState(NPCState state) //状态转换
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };

        currentState.OnExit(); //退出状态
        currentState = newState; //变更
        currentState.OnEnter(this); //进入新状态
    
    }

    public virtual void OnDrawGizmosSelected() //画出检测玩家范围
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset + new Vector3(checkDistance*-transform.localScale.x, 0),0.2f);
    
    }
}
