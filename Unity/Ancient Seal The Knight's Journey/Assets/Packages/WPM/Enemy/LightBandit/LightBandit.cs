using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WPM
{
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

        public Vector2 attackRange = new(1f, 0.3f);
        public float attackDamage = 5f;

        #endregion

        void Awake()
        {
            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (isAlive)
            {
                var player = TrackPlayer();
                if (player != null) { AttackPlayer(player); }
            }
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
                if (cooldown <= 0)
                {
                    var range = RangeToPhysicsPoints(attackRange);
                    var playerHitbox =
                        Physics2D.OverlapAreaAll(range.Item1, range.Item2)
                        .Where(x => x.CompareTag("Player"))
                        .FirstOrDefault();

                    if (playerHitbox != null)
                    {
                        cooldown += 1.2f;
                        StartCoroutine(StartAttack());
                    }
                }
            }
        }
        IEnumerator StartAttack()
        {
            while (true && isAlive)
            {
                yield return new WaitForSeconds(attackDelay);

                if (isAlive)
                    AttackAnim();
                yield return new WaitForSeconds(attackDelay);

                if (isAlive)
                    Attack();

                break;
            }
        }
        void AttackAnim()
        {
            animator.SetTrigger("Attack");
        }
        void Attack()
        {
            var range = RangeToPhysicsPoints(attackRange);
            var playerHitbox =
                Physics2D.OverlapAreaAll(range.Item1, range.Item2)
                .Where(x => x.CompareTag("Player"))
                .FirstOrDefault();
            if (playerHitbox != null)
            {
                playerHitbox.SendMessage("TakenDamage", attackDamage);
            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            // Debug Basic Attack range
            {
                Gizmos.color = Color.yellow;
                var pp = RangeToPhysicsPoints(attackRange);
                var cr = PhysicsPointsToCenterRect(pp);
                Gizmos.DrawWireCube(cr.Item1, cr.Item2);
            }
        }
    }
}