using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator animatior;
    public abstract bool isAlive { get; protected set; }

    public abstract float health { get; protected set; }
    public abstract float healthMax { get; protected set; }
    public abstract float healthRegen { get; protected set; }

    public abstract float armor { get; protected set; }
    public abstract Dictionary<string, float> armorMul { get; protected set; }

    public abstract float mana { get; protected set; }
    public abstract float manaMax { get; protected set; }
    public abstract float manaRegen { get; protected set; }

    public abstract float invincibleTime { get; protected set; }
    public abstract float injuredTimer { get; protected set; }

    public abstract float cooldownBasic { get; protected set; }
    public abstract float cooldown { get; protected set; }

    public abstract float stunTimer { get; protected set; }

    public float destoryTimer { get; protected set; } = 3f;

    void Awake()
    {
        animatior = GetComponent<Animator>();
    }

    public void TakenDamage(float damage)
    {
        // Taken damage
        var takenDamage = damage - armor;
        foreach (var armor_m in armorMul) { takenDamage *= armor_m.Value; }
        takenDamage = takenDamage <= 0 ? 1 : takenDamage;
        health -= takenDamage;

        animatior.SetTrigger("Hurt");

        if (health <= 0)
        {
            
            health = 0;
            isAlive = false;
        }
        // Injured
        else
        {
            // 
        }
    }
    public void GetStunned(float duration)
    {
        stunTimer = duration;
    }
    public void Stun()
    {
        if (stunTimer > 0) { stunTimer -= Time.deltaTime; }
    }
    public void Death()
    {
        
       

        // When a character is dead, it will be destroyed after 3 seconds.
        if (!isAlive) 
        {
            // Start death animation
            animatior.SetTrigger("Death");

            // Destory countdown
            destoryTimer -= Time.deltaTime; 
        }
        if (destoryTimer <= 0) { Destroy(gameObject); }
    }
    void FixedUpdate()
    {
        Stun();
        Death();
    }
}
