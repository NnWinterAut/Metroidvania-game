using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class SnailSkillState : BaseState
    {
        public override void OnEnter(Enemy enemy)
        {
            currentEnemy = enemy; //初始化当前敌人
            currentEnemy.lostTimeCounter = currentEnemy.lostTime; //重置时间
            currentEnemy.currentSpeed = 0;

            currentEnemy.anim.SetBool("walk", false); //躲藏动画
            currentEnemy.anim.SetBool("hide", true);
            currentEnemy.anim.SetTrigger("skill");

            currentEnemy.GetComponent<Character>().invulnerable = true; //进入无敌
            currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter; //退出时间等于无敌时间

        }

        public override void LogicUpdate()
        {
            if (currentEnemy.lostTimeCounter <= 0)
            {
                currentEnemy.SwitchState(NPCState.Patrol);
            }

            currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter; //退出时间会一直等于无敌时间

        }

        public override void PhysicsUpdate()
        {

        }

        public override void OnExit()
        {
            currentEnemy.anim.SetBool("hide", false);
            currentEnemy.GetComponent<Character>().invulnerable = false; //退出无敌
        }

    }
}