using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("无敌时间")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    private void Start() {

        currentHealth = maxHealth;
    }

    private void Update() {

        if (invulnerable) {

            invulnerableCounter -= Time.deltaTime; //开始2秒倒计时
            if (invulnerableCounter <= 0) {

                invulnerable = false; //可以继续受到伤害
            }
        }
    
    }

    public void TakeDamage(Attack attacker) //受到伤害
    {
        if (invulnerable) {
            return;
        }
        if (currentHealth - attacker.damage > 0) //血量足够厚
        {
            currentHealth -= attacker.damage; //瞬间受到伤害
            TriggerInvulnerable(); //激活触发器, 激活无敌
        }
        else {
            currentHealth = 0; //人物死亡;
         
        }


    }

    public void TriggerInvulnerable() {

        if (invulnerable == false) {
            invulnerable = true; //开始在Update中计时
            invulnerableCounter = invulnerableDuration; //计时开始, 2秒无敌
        }
    
    }
}
