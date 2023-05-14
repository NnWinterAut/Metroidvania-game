using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private Animator animator;
    private Rigidbody2D rigid;

    public float health;
    public float health_max;
    public float magic;
    public float magic_max;

    public float armor;
    public GameObject[] loots;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void TakenDamage(float damage)
    {
        Debug.Log($"TakenDamage:{damage}");
        health -= damage;
        if (health < 0)
        {
            Death();
        }
    }
    void Death()
    {
        Destroy(gameObject);
    }
}
