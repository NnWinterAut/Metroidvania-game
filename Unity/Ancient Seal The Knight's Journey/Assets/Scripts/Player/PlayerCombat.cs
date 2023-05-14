using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Character
{
    public override bool isAlive { get; protected set; } = true;
    public override float health { get; protected set; } = 100.0f;
    public override float healthMax { get; protected set; } = 100.0f;
    public override float healthRegen { get; protected set; } = 0.0f;
    public override float armor { get; protected set; } = 0.0f;
    public override Dictionary<string, float> armorMul { get; protected set; } = new ();
    public override float mana { get; protected set; } = 100.0f;
    public override float manaMax { get; protected set; } = 100.0f;
    public override float manaRegen { get; protected set; } = 5.0f;
    public override bool isInjuring { get; protected set; } = false;
    public override float invincibleTime { get; protected set; } = 0.5f;
    public override float injuredTimer { get; protected set; } = 0.25f;

    void Start()
    {

    }

    void Update()
    {

    }

}
