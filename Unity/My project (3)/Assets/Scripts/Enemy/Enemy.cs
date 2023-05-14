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

    public bool isAlive = true;

    public float armor_abs = 0f;
    public Dictionary<string,float> armor_mul = new();
    public GameObject[] loots;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void TakenDamage(float damage)
    {
        var taken = damage - armor_abs;
        foreach(var armor_m in armor_mul.Values) { taken *= armor_m; }

        health -= taken;
        if (health <= 0.5f)
        {
            isAlive = false;
            Death();
        }
    }
    protected void Death()
    {
        Destroy(gameObject);
    }
}
