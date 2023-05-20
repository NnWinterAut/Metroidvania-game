using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chenhao
{
    public class Boar : Enemy
    {
        public override void Move()
        {
            base.Move();
            anim.SetBool("walk", true);
        }
    }
}