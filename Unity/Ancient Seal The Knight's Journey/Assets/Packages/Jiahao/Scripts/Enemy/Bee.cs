using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class Bee : Enemy
    {
        [Header("�ƶ���Χ")]
        public float patrolRadius;

        protected override void Awake()
        {
            base.Awake();
            patrolState = new BeePatrolState();
            chaseState = new BeeChaseState();
        }
        public override bool FoundPlayer() //������
        {
            var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer); //����Բ�μ�����

            if (obj)
            {
                attacker = obj.transform;
            }

            return obj;
        }

        public override Vector3 GetNewPoint()
        {
            var targetX = Random.Range(-patrolRadius, patrolRadius); //���X��
            var targetY = Random.Range(-patrolRadius, patrolRadius); //���Y��

            return spwanPoint + new Vector3(targetX, targetY);
        }

        public override void Move() //��д�ƶ�
        {

        }

        public override void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, checkDistance);
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
    }
}