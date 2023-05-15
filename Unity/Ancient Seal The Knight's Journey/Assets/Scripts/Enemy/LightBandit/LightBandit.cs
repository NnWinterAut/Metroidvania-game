using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class LightBandit : Enemy
{
    #region ---- Params ----

    public override Animator animator { get; protected set; }
    public override Rigidbody2D rigid { get; protected set; }
    public override Collider2D col { get; protected set; }

    public override bool isAlive { get; protected set; } = true;

    public override float health { get; protected set; } = 5f;
    public override float healthMax { get; protected set; } = 100f;
    public override float healthRegen { get; protected set; } = 0f;

    public override float armor { get; protected set; } = 5f;
    public override Dictionary<string, float> armorMul { get; protected set; } = new();

    public override float mana { get; protected set; } = 0f;
    public override float manaMax { get; protected set; } = 0f;
    public override float manaRegen { get; protected set; } = 0f;

    public override float invincibleTime { get; protected set; } = 0.5f;
    public override float injuredTimer { get; protected set; } = 0f;

    public override float cooldown { get; protected set; } = 0f;

    public override float stunTimer { get; protected set; } = 0f;

    public override List<GameObject> loots { get; protected set; } = new();
    public override float detectionSphere { get; protected set; } = 2.5f;
    public override Rect detectionRectangle { get; protected set; } = new Rect(4, 0, 8, 5);
    public override Vector2 speed { get; protected set; } = new Vector2(1.5f, 0f);

    public override bool isAlerted { get; protected set; } = false;
    public override float attackDelay { get; protected set; } = 0.4f;

    #endregion

    #region ---- Local Params ----

    public Vector2 attackRange = new(0.6f, 0.3f);

    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        var player = TrackPlayer();
        if (player != null) { AttackPlayer(player); }
    }

    #region ---- Track Player ----
    Collider2D DetectPlayer()
    {
        var player = PlayerDetector_Rect();
        if (player != null) { return player; }

        if (isAlerted)
        {
            player = PlayerDetector_Sphere();
        }
        return player;
    }
    Collider2D TrackPlayer()
    {
        var player = DetectPlayer();
        if (player != null)
        {
            isAlerted = true;
            FollowPlayer(player);
        }
        else
        {
            isAlerted = false;
        }
        return player;
    }

    void FollowPlayer(Collider2D player)
    {
        var pCenter = player.bounds.center;
        var center = rigid.worldCenterOfMass;

        var dir = pCenter.x < center.x;
        var move = dir ? new Vector2(-1, 0) : new Vector2(1, 0);

        if (move.x < 0 && IsFacingRight() || move.x > 0 && !IsFacingRight())
        {
            var rotation = transform.eulerAngles;
            rotation.y = (rotation.y + 180f) % 360;
            transform.eulerAngles = rotation;
        }

        if (Mathf.Abs(pCenter.x - center.x) > (col.bounds.size.x + player.bounds.size.x))
        {
            Move(move);
        }
    }

    #endregion

    #region ---- Attack ----

    void AttackPlayer(Collider2D player)
    {
        if (isAlerted)
        {
            var playerLoc = player.bounds.center;
            var yLen = Mathf.Abs(playerLoc.y - col.bounds.center.y);
            var xLen = Mathf.Abs(playerLoc.x - col.bounds.center.x);
            if (yLen < attackRange.y && xLen < attackRange.x && cooldown <= 0)
            {
                cooldown += 1.2f;
                StartCoroutine(StartAttack());
            }
        }
    }
    IEnumerator StartAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackDelay);
            Attack();
            yield return new WaitForSeconds(attackDelay);
            break;
        }
        
        
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
    }
    #endregion
}
