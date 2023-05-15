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
    public override float detectionSphere { get; protected set; } = 0f;
    public override Sector detectionSector { get; protected set; } = new Sector(0,0);
    public override Rect detectionRectangle { get; protected set; } = new Rect(0,0,10,10);
    public override float speed { get; protected set; } = 2f;

    public override bool isTrackingPlayer { get; protected set; } = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Detector();
    }
}
