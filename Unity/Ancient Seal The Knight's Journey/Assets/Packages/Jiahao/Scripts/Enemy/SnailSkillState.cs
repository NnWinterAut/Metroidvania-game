using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class SnailSkillState : BaseState
    {
        public override void OnEnter(Enemy enemy)
        {
            currentEnemy = enemy; //��ʼ����ǰ����
            currentEnemy.lostTimeCounter = currentEnemy.lostTime; //����ʱ��
            currentEnemy.currentSpeed = 0;

            currentEnemy.anim.SetBool("walk", false); //��ض���
            currentEnemy.anim.SetBool("hide", true);
            currentEnemy.anim.SetTrigger("skill");

            currentEnemy.GetComponent<Character>().invulnerable = true; //�����޵�
            currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter; //�˳�ʱ������޵�ʱ��

        }

        public override void LogicUpdate()
        {
            if (currentEnemy.lostTimeCounter <= 0)
            {
                currentEnemy.SwitchState(NPCState.Patrol);
            }

            currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter; //�˳�ʱ���һֱ�����޵�ʱ��

        }

        public override void PhysicsUpdate()
        {

        }

        public override void OnExit()
        {
            currentEnemy.anim.SetBool("hide", false);
            currentEnemy.GetComponent<Character>().invulnerable = false; //�˳��޵�
        }

    }
}