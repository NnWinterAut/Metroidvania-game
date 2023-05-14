using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigi;

    public bool isAttack;

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    private void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            isAttack = true;
            animator.SetTrigger("Attack");
        }
        else
        {
            isAttack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (transform.localScale.x > 0)
            {
                collision.GetComponent<Enemy>().GetEnemyHit(Vector2.right);
            }
            else if(transform.localScale.x<0)
            {
                collision.GetComponent<Enemy>().GetEnemyHit(Vector2.left);
            }
        }
    }
}
