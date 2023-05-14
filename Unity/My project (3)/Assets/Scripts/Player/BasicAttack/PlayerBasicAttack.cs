using System.Timers;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownTimer.Elapsed += (sender, e) => { canAttack = true; };
    }
    public float cooldown = 0.5f;
    public bool canAttack = true;
    private Timer cooldownTimer = new Timer();
    void Update()
    {
        if (Input.GetButtonDown("BasicAttack") && canAttack)
        {
            animator.SetTrigger("BasicAttack");
            canAttack = false;
            cooldownTimer.Interval = cooldown;
            cooldownTimer.Start();
            DoBasicAttack();
        }
    }
    public float damage = 1.0f;
    public Transform pos;
    public LayerMask enemiesLayerMask;
    public float range = 100.0f;
    void DoBasicAttack()
    {
        Debug.Log($"Send Damage @ {pos.position}");

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(pos.position, range, enemiesLayerMask);
        foreach(var enemy in enemiesInRange)
        {
            enemy.gameObject.SendMessage("TakenDamage",damage);
            
        }
    }
}
