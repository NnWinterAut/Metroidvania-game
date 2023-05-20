using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;

    public bool manual;

    //Check the player is on the ground
    public bool isGround;

    public bool touchLeftWall;
    public bool touchRightWall;

    //Dectection range
    public float checkRaduis;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public LayerMask groundLayer;


    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x)/2,coll.bounds.size.y/2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //Check ground
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);

        //Check wall
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        touchRightWall= Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);

    }
}
