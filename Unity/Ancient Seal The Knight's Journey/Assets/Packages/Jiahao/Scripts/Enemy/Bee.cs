using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class Bee : Enemy
    {
        [Header("移动范围")]
        public float patrolRadius;

        protected override void Awake()
        {
            base.Awake();
            patrolState = new BeePatrolState();
            chaseState = new BeeChaseState();
        }
        public override bool FoundPlayer() //检测玩家
        {
            var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer); //敌人圆形检测玩家

            if (obj)
            {
                attacker = obj.transform;
            }

            return obj;
        }

        public override Vector3 GetNewPoint()
        {
            var targetX = Random.Range(-patrolRadius, patrolRadius); //随机X点
            var targetY = Random.Range(-patrolRadius, patrolRadius); //随机Y点

            return spwanPoint + new Vector3(targetX, targetY);
        }

        public override void Move() //复写移动
        {

        }

        public override void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, checkDistance);
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
    }
}