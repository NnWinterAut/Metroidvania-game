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
    public Transform leftPoint, rightPoint;
    private Animator animator;
    public Transform Cast; //ray origin
    public LayerMask CastMask; //Ray Layer Variables

    private RaycastHit2D hit; //projected information variables
    private Transform target; //Player Target Information
    #endregion

    #region game variables
    public float moveSpeed; // enemy movement speed
    public float CastLength; //Ray length
    public float attackDistance; //attack minimum distance
    public float timer; //cooldown of enemy attacks

    private bool attackModel; //Enemy enters attack mode
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
            animator.SetBool("CanWalk", true);
        //Get the current animation state, make sure there is no attack animation
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
            //storage destination
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            //Enemy current position, target position, movement speed
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
        if (Range)
        {
            //emit rays and store
            hit = Physics2D.Raycast(Cast.position, transform.right, CastLength, CastMask);
            CastDebugger();
        }
        if (Range == false)
        {
            StopAttack();
        }
        //when player detected
        //Cannot move while attacking
        if (!attackModel)
        {
            move();
        }
        ///When the enemy is out of bounds and the player is out of range and the enemy is not attacking
        if (!InsideofLimits() && !Range && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            ObjectSelectorTargetInfo();
        }
        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            Range = false;
        }
    }
    //Whether the player entered the trigger zone
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            //Storing game objects to variables
            target = trigger.transform;
            Range = true;
            Flip();
        }
    }

    private void EnemyLogic()
    {
        //Store and calculate the distance between the enemy and the player
        distance = Vector2.Distance(transform.position, target.position);
        //If distance is greater than the attack range
        if (distance > attackDistance)
        {
            StopAttack();
        }
        //Attack distance greater or equal to the distance and the attack is not on cooldown
        else if (attackDistance >= distance && cooling == false){
            Attack();
        }
        //stop attack animation
        if (cooling)
        {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }

    //Rays seen in the scene
    private void CastDebugger()
    {
        //Check if the enemy player distance is greater than the attack distance
        if (distance > attackDistance)
        {
            //Draw rays, ray lengths and variables
            Debug.DrawRay(Cast.position, transform.right * CastLength, Color.red);
        }
        //Attack distance is greater than check enemy player distance
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

    //the enemy on the border
    private bool InsideofLimits()
    {
        //Check if the enemy's position is greater than the left border or less
        return transform.position.x > leftPoint.position.x&&transform.position.x< rightPoint.position.x;
    }

    //Make the enemy move to the target
    private void ObjectSelectorTargetInfo()
    {
        //Calculate the distance from the enemy to the left border and borde
        float distanceToLeft = Vector2.Distance(transform.position, leftPoint.position);
        float distanceToRight = Vector2.Distance(transform.position, rightPoint.position);

        //If the left border distance greater than the bounded distance, set to the left limit
        if (distanceToLeft > distanceToRight)
        {
            target = leftPoint;
        }
        else//right limit
        {
            target = rightPoint;
        }
        Flip();
    }

    //flip enemy
    private void Flip()
    {
        //store flip angle
        Vector3 rotation = transform.eulerAngles;
        //Check if the enemy position is larger than the target position
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        //initial angle
        transform.eulerAngles = rotation;
    }
}


