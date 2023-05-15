using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Animator animatior;
    protected Rigidbody2D rigid;
    protected Collider2D col;

    #region ---- Movement params ----

    public abstract Vector2 speed { get; protected set; }

    #endregion

    #region ---- Combat params ----

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

    #endregion

    void Awake()
    {
        animatior = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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

            // Start death animation
            animatior.SetTrigger("Death");

            // Disable collider and rigid to lock dead body
            col.enabled = false;
            rigid.simulated = false;
        }
    }
    public void GetStunned(float duration)
    {
        stunTimer = duration;
    }
    protected void Stun()
    {
        if (stunTimer > 0) { stunTimer -= Time.deltaTime; }
    }
    public void Death()
    {
        // When a character is dead, it will be destroyed after 3 seconds.
        if (!isAlive) 
        {
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

    protected bool IsFacingRight()
    {
        return transform.eulerAngles.y < 90f && transform.eulerAngles.y > -90f;
    }
}
