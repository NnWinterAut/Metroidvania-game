using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigi;
    public SpriteRenderer sr;
    public Transform leftPoint, rigitPoint;
    private Animator animator;
    public Transform Cast;
    public LayerMask CastMask;
    //Ͷ�䷶Χ
    private RaycastHit2D hit;
    private GameObject target;

    public float moveSpeed;
    public float moveTime, waitTime;
    private float moveCount, waitCount;
    public float CastLength;
    public float attackDistance;
    public float timer; //cooldown of attacks
    private bool attack;
    private float distance; //Stroe the distance b/w enemy and player
    private bool Range; //check if player in range
    private bool cooling;//check if Enemy is cooling after attack
    private float intTimer;

    private bool movingRight;
    void Start()
    {
        //rigi=GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();

        //movingRight = false;
        //moveCount = moveTime;

        //intTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        //move();
        //RangeToAttack();
        //Attack();
        //StopAttack();
    }
    //private void move()
    //{
    //    if (moveCount > 0)
    //    {
    //        moveCount -= Time.deltaTime;
    //        if (movingRight)
    //        {
    //            rigi.velocity = new Vector2(moveSpeed, rigi.velocity.y);
    //            sr.flipX = true;
    //            if (transform.position.x > rigitPoint.position.x)
    //            {
    //                movingRight = false;
    //            }
    //        }
    //        else
    //        {
    //            rigi.velocity = new Vector2(-moveSpeed, rigi.velocity.y);
    //            sr.flipX = false;
    //            if (transform.position.x < leftPoint.position.x)
    //            {
    //                movingRight = true;
    //            }
    //        }
    //        if (moveCount <= 0)
    //        {
    //            waitCount = Random.Range(waitTime * .70f, waitTime * 1.25f);
    //        }
    //        animator.SetInteger("AnimState", 2);
    //        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("En_attack"))
    //        {
    //            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
    //            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    //        }
    //    }
    //    //else if (waitCount > 0)
    //    //{
    //    //    waitCount -= Time.deltaTime;
    //    //    rigi.velocity = new Vector2(0f, rigi.velocity.y);
    //    //    if (waitCount <= 0)
    //    //    {
    //    //        moveCount = Random.Range(moveTime * .70f, waitTime * .70f);
    //    //    }
    //    //    animator.SetInteger("AnimState", 1);
    //    //}
    //}


    //private void Attack()
    //{
    //    timer = intTimer;
    //    attack = true;

    //    animator.SetBool("Run", false);
    //    animator.SetBool("Attack", true);
    //}
    //private void StopAttack()
    //{
    //    cooling = false;
    //    attack = false;
    //    animator.SetBool("Attack", false);
    //}

    //private void RangeToAttack()
    //{
    //    if (Range)
    //    {
    //        hit = Physics2D.Raycast(Cast.position, Vector2.left, CastLength, CastMask);
    //        CastDebugger();
    //    }
    //    //when player detected
    //    if(hit.collider != null)
    //    {
    //        EnemyLogic();
    //    }
    //    else if(hit.collider == null)
    //    {
    //        Range = false;
    //    }
    //    if (Range == false)
    //    {
    //        // animator.SetBool("Walk", false);
    //        StopAttack();
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        target = collision.gameObject;
    //        Range = true;
    //    }
    //}

    //private void EnemyLogic()
    //{
    //    distance = Vector2.Distance(transform.position, target.transform.position);
    //    if (distance > attackDistance)
    //    {
    //        move();
    //        StopAttack();
    //    }
    //    else if(attackDistance >= distance && cooling == false){
    //        Attack();
    //    }
    //    if (cooling)
    //    {
    //        Cooldown();
    //        animator.SetBool("Attack", false);
    //    }
    //}
    //private void CastDebugger()
    //{
    //    if (distance > attackDistance)
    //    {
    //        Debug.DrawRay(Cast.position, Vector2.left * CastLength, Color.red);
    //    }
    //    else if (attackDistance > distance)
    //    {
    //        Debug.DrawRay(Cast.position, Vector2.left * CastLength, Color.green);
    //    }
    //}

    //public void TriggerCooling()
    //{
    //    cooling = true;
    //}

    //void Cooldown()
    //{
    //    timer -= Time.deltaTime;
    //    if (timer <= 0 && cooling && attack)
    //    {
    //        cooling = false;
    //        timer = intTimer;
    //    }
    //}
}
