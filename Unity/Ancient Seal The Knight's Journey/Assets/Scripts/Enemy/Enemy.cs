using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : Character
{
    public abstract List<GameObject> loots { get; protected set; }
    public abstract float detectionSphere { get; protected set; }
    public abstract Rect detectionRectangle { get; protected set; }

    public abstract bool isAlerted { get; protected set; }
    public abstract float attackDelay { get; protected set; }

    #region ---- Movement ----

    protected void Move(Vector2 direction)
    {
        rigid.velocity = speed * direction;
    }

    #endregion

    #region ---- Detector ----

    protected Collider2D PlayerDetector_Sphere()
    {
        var s_player = Physics2D.OverlapCircleAll(rigid.worldCenterOfMass, detectionSphere).Where(x => x.CompareTag("Player"));
        return s_player.FirstOrDefault();
    }
    protected Collider2D PlayerDetector_Rect()
    {
        var r_detect = GetDetectorRectangle();
        var r_player = Physics2D.OverlapAreaAll(r_detect.Item1, r_detect.Item2).Where(x => x.CompareTag("Player"));
        return r_player.FirstOrDefault();
    }
    (Vector2,Vector2) GetDetectorRectangle()
    {
        if (rigid.IsUnityNull()) { rigid = gameObject.GetComponent<Rigidbody2D>(); }

        var center = rigid.worldCenterOfMass;

        var p1 = new Vector2(
                center.x - detectionRectangle.width / 2,
                center.y - detectionRectangle.height / 2
                );
        var p2 = new Vector2(
                center.x + detectionRectangle.width / 2,
                center.y + detectionRectangle.height / 2
                );

        var shift = new Vector2(
            IsFacingRight() ? detectionRectangle.x : - detectionRectangle.x,
            detectionRectangle.y
            );

        p1 += shift;
        p2 += shift;

        return (new Vector2(p1.x, p1.y), new Vector2(p2.x, p2.y));
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        // ---- Detector ----

        Gizmos.color = Color.yellow;
        var rec = GetDetectorRectangle();
        var center = new Vector2((rec.Item1.x + rec.Item2.x) / 2, (rec.Item1.y + rec.Item2.y) / 2);
        var size = new Vector2(Mathf.Abs(rec.Item2.x - rec.Item1.x), Mathf.Abs(rec.Item2.y - rec.Item1.y));

        Gizmos.DrawWireCube(center, size);

        Gizmos.DrawWireSphere(rigid.worldCenterOfMass, detectionSphere);

        // ----
    }
}

public struct Sector
{
    public float Range;
    public float Angle;
    public Sector(float range, float angle)
    {
        Range = range;
        Angle = angle;
    }
}