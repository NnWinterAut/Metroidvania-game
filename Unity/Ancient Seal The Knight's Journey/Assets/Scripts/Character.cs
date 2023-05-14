using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract bool isAlive { get; protected set; }

    public abstract float health { get; protected set; }
    public abstract float healthMax { get; protected set; }
    public abstract float healthRegen { get; protected set; }

    public abstract float armor { get; protected set; }
    public abstract Dictionary<string, float> armorMul { get; protected set; }

    public abstract float mana { get; protected set; }
    public abstract float manaMax { get; protected set; }
    public abstract float manaRegen { get; protected set; }

    public abstract bool isInjuring { get; protected set; }
    public abstract float invincibleTime { get; protected set; }
    public abstract float injuredTimer { get; protected set; }

    public abstract float cooldownBasic { get; protected set; }
    public abstract float cooldown { get; protected set; }

    public abstract float stunTimer { get; protected set; }

    public void TakenDamage(float damage)
    {
        Debug.Log($"HP = {health}");

        var takenDamage = damage - armor;
        foreach (var armor_m in armorMul) { takenDamage *= armor_m.Value; }
        takenDamage = takenDamage <= 0 ? 1 : takenDamage;
        health -= takenDamage;

        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public void GetStunned(float duration)
    {
        stunTimer = duration;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if(stunTimer > 0) { stunTimer -= Time.deltaTime; }
    }
}
