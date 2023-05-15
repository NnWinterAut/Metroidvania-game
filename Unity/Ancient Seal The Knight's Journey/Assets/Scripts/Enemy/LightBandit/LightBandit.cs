using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LightBandit : Enemy
{
    #region ---- Params ----

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

    public override float cooldownBasic { get; protected set; } = 1.5f;
    public override float cooldown { get; protected set; } = 0f;

    public override float stunTimer { get; protected set; } = 0f;

    public override List<GameObject> loots { get; protected set; } = new();
    public override float detectionSphere { get; protected set; } = 2.5f;
    public override Rect detectionRectangle { get; protected set; } = new Rect(4,0,8,5);
    public override Vector2 speed { get; protected set; } = new Vector2(1.5f,0f);

    public override bool isAlerted { get; protected set; } = false;

    #endregion

    #region ---- Local Params ----

    public Vector2 attackRange = new (0.6f, 0.3f);

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var player = TrackPlayer();
    }

    #region ---- Track Player ----
    Collider2D DetectPlayer()
    {
        var player = PlayerDetector_Rect();
        if(player != null) { return player; }

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

        if (Mathf.Abs(pCenter.x - center.x)>(col.bounds.size.x + player.bounds.size.x))
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
            var yLen = playerLoc.y - col.bounds.center.y;
            var xLen = playerLoc.x - col.bounds.center.x;
            if (yLen < attackRange.y && xLen < attackRange.x)
            {
                StartCoroutine(StartAttack());
            }
        }
    }
    IEnumerator StartAttack()
    {
        while (true)
        {
            if (!isAttacking)
            {
                // 进入等待状态
                yield return new WaitForSeconds(attackDelay);

                // 开始攻击
                isAttacking = true;
                Attack();

                // 攻击完成后退出等待状态
                isAttacking = false;
            }

            yield return null;
        }
    }

    #endregion
}
