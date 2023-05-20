using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public class Boar : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            patrolState = new BoarPatrolState();
            chaseState = new BoarChaseState();

        }

    }
}