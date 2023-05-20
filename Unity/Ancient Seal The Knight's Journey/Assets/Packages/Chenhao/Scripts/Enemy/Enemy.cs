using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chenhao
{
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public Animator anim;

        PhysicsCheck physicsCheck;

        [Header("Basic attribute")]
        public float normalSpeed;
        public float chaseSpeed;
        public float currentSpeed;
        public Vector3 faceDir;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            physicsCheck = GetComponent<PhysicsCheck>();
            currentSpeed = normalSpeed;
        }

        private void Update()
        {
            faceDir = new Vector3(-transform.localScale.x, 0, 0);

            if (physicsCheck.touchLeftWall || physicsCheck.touchRightWall)
            {
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        private void FixedUpdate()
        {
            Move();
        }
        public virtual void Move()
        {
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
    }
}