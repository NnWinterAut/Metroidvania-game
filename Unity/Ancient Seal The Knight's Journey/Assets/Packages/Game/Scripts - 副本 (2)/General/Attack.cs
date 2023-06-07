using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D other) //人物碰撞触发器
    {
        other.GetComponent<Character>()?.TakeDamage(this); //受到伤害, ?不为空的判断 !=null
    }
}