using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 direction;
    public AnimatorStateInfo info;
    public Transform target;
    private Animator enemyanimator;
    private Rigidbody2D enemyrigi;

    public float enemyHP;
    public float enemyMaxHP;
    bool isHit;

    void Start()
    {
        enemyanimator = GetComponent<Animator>();
        enemyrigi = GetComponent<Rigidbody2D>();
        enemyHP = enemyMaxHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHit();
    }

    void EnemyHit()
    {
        if (!isHit)
        {
            GetComponent<EnemyController>().moveSpeed = 0;
            if (info.normalizedTime >= .6)
            {
                isHit = false;
                GetComponent<EnemyController>().moveSpeed = 1;
            }
        }
    }

    public void GetEnemyHit(Vector2 direction)
    {
        if (!isHit)
        {
            transform.localScale = new Vector3(direction.x, 1, 1);
            isHit = true;
            this.direction = -direction;
            enemyanimator.SetTrigger("hit");
        }
    }
}
