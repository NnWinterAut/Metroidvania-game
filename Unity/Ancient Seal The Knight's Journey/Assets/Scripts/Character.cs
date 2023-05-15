using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class Character : MonoBehaviour
{
    public abstract Animator animator { get; protected set; }
    public abstract Rigidbody2D rigid { get; protected set; }
    public abstract Collider2D col { get; protected set; }

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

    public abstract float cooldown { get; protected set; }

    public abstract float stunTimer { get; protected set; }

    public float destoryTimer { get; protected set; } = 3f;

    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Stun();
        Death();
        Cooldown();
    }

    public void TakenDamage(float damage)
    {
        // Taken damage
        var takenDamage = damage - armor;
        foreach (var armor_m in armorMul) { takenDamage *= armor_m.Value; }
        takenDamage = takenDamage <= 0 ? 1 : takenDamage;
        health -= takenDamage;

        animator.SetTrigger("Hurt");

        // Need to replace this to somewhere else
        {
            var enemy = GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SendMessage("Alert");
            }
        }

        if (health <= 0)
        {
            health = 0;
            isAlive = false;

            // Start death animation
            animator.SetTrigger("Death");

            // Disable collider and rigid to lock dead body
            col.enabled = false;
            rigid.simulated = false;
        }
    }
    public void SetStunned(float duration)
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

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y < 90f && transform.eulerAngles.y > -90f;
    }

    public void AddCooldown(float cooldown)
    {
        this.cooldown += cooldown;
    }
    private void Cooldown()
    {
        if (cooldown >= 0) { 
            cooldown -= Time.deltaTime;
            if(cooldown < 0) { cooldown = 0; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <returns>(TopFar, BotClose)</returns>
    protected (Vector2,Vector2) RangeToPhysicsPoints(Vector2 range)
    {
        if (rigid == null) { rigid = GetComponent<Rigidbody2D>(); }
        var center = rigid.worldCenterOfMass;
        var isRight = IsFacingRight();
        var halfModel = GetComponent<Collider2D>().bounds.size.x / 2;

        var edge = isRight ? halfModel : -halfModel;
        var rangex = isRight ? range.x : -range.x;

        var p1 = new Vector2(center.x + edge + rangex, center.y + range.y / 2);

        var p2 = new Vector2(center.x + edge, center.y - range.y / 2);

        return (p1, p2);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="points"></param>
    /// <returns>(Center, Size)</returns>
    protected (Vector2,Vector2) PhysicsPointsToCenterRect((Vector2, Vector2) points)
    {
        return (
            new Vector2(
            (points.Item1.x + points.Item2.x) / 2,
            (points.Item1.y + points.Item2.y) / 2)
            ,
            (points.Item2 - points.Item1).Abs()
            );
    }
}
