using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class BeeChaseState : BaseState
    {
        private Vector3 target;
        private Vector3 moveDir;

        private Attack attack;
        private bool isAttack;
        private float attackRateCounter = 0; //飞行敌人

        public override void OnEnter(Enemy enemy)
        {
            currentEnemy = enemy;
            currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
            attack = enemy.GetComponent<Attack>();

            currentEnemy.lostTimeCounter = currentEnemy.lostTime;
            currentEnemy.anim.SetBool("chase", true);
        }

        public override void LogicUpdate()
        {
            if (currentEnemy.lostTimeCounter <= 0)
            {
                currentEnemy.SwitchState(NPCState.Patrol);
            }

            //计时器
            attackRateCounter -= Time.deltaTime;

            target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0); //将目标地点改为玩家位置, +1.5f为上半身判定

            //判断攻击距离
            if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
            {
                isAttack = true;

                if (!currentEnemy.isHurt)
                {

                    currentEnemy.rb.velocity = Vector2.zero;

                }

                if (attackRateCounter <= 0)
                {

                    currentEnemy.anim.SetTrigger("attack");
                    attackRateCounter = attack.attackRate;

                }
            }
            else    //超出攻击范围
            {
                isAttack = false;
            }

            moveDir = (target - currentEnemy.transform.position).normalized; //敌人朝向

            if (moveDir.x > 0)
            {
                currentEnemy.transform.localScale = new Vector3(-1, 1, 1);

            }
            if (moveDir.x < 0)
            {
                currentEnemy.transform.localScale = new Vector3(1, 1, 1);
            }

        }

        public override void PhysicsUpdate()
        {
            if (!currentEnemy.isHurt && !currentEnemy.isDead && !isAttack)
            {
                currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
            }
        }
        public override void OnExit()
        {
            currentEnemy.anim.SetBool("chase", false);
        }

    }
}