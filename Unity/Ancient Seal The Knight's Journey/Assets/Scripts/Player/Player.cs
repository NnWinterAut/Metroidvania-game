using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override Animator animator { get; protected set; }
    public override Rigidbody2D rigid { get; protected set; }
    public override Collider2D col { get; protected set; }

    public override bool isAlive { get; protected set; } = true;

    public override float health { get; protected set; } = 100.0f;
    public override float healthMax { get; protected set; } = 100.0f;
    public override float healthRegen { get; protected set; } = 0.0f;

    public override float armor { get; protected set; } = 0.0f;
    public override Dictionary<string, float> armorMul { get; protected set; } = new ();

    public override float mana { get; protected set; } = 100.0f;
    public override float manaMax { get; protected set; } = 100.0f;
    public override float manaRegen { get; protected set; } = 5.0f;

    public override float invincibleTime { get; protected set; } = 0.5f;
    public override float injuredTimer { get; protected set; } = 0f;

    public override float cooldown { get; protected set; } = 0.0f;

    public override float stunTimer { get; protected set; } = 0.0f;
    public override Vector2 speed { get; protected set; } = new Vector2(2.0f,0f);

    void Start()
    {

    }

    void Update()
    {

    }
}
