using System.Linq;
using System.Timers;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public float cooldown = 0.5f;
    public bool canAttack = true;
    private float timer = 0.0f;
    void Update()
    {
        BasicAttack();
    }
    /// <summary>
    /// Check if player can do a basic attack
    /// </summary>
    void BasicAttack()
    {
        if (Input.GetButtonDown("BasicAttack") && canAttack)
        {
            animator.SetTrigger("BasicAttack");
            canAttack = false;
            DoBasicAttack();
        }
        else if (!canAttack)
        {
            timer += Time.deltaTime;
            // Check if cooldown ends
            if (timer >= cooldown) 
            {
                // Allow player to attack and reset timer
                canAttack = true; 
                timer = 0.0f; 
            }
        }
    }
    public float damage = 1.0f;
    public Vector2 pos = new(0.08f, -2.6f);
    public Vector2 range = new(1.06f, 1.06f);
    /// <summary>
    /// Do a basic attack
    /// </summary>
    void DoBasicAttack()
    {
        var attack_pos = GetAttackPos();
        var enemiesInRange = Physics2D.OverlapAreaAll(attack_pos.Item1, attack_pos.Item2).Where(x => x.CompareTag("Enemy"));
        foreach (var enemy in enemiesInRange)
        {
            enemy.gameObject.SendMessage("TakenDamage", damage);
        }
    }
    /// <summary>
    /// Get the pos of the attack
    /// </summary>
    /// <returns>Vector of the attack's center position</returns>
    private (Vector2, Vector2) GetAttackPos()
    {
        var p_pos = transform.position;

        var x0 = p_pos.x + (transform.rotation.y >= 0 ? pos.x : -(pos.x + GetComponent<Collider2D>().bounds.size.x*2.5f));
        var y0 = p_pos.y + pos.y;
        var x1 = x0 + range.x;
        var y1 = y0 + range.y;
        return (new Vector2(x0,y0),new Vector2(x1,y1));
    }
    private void OnDrawGizmos()
    {
        // Debug Basic Attack range
        {
            Gizmos.color = Color.yellow;
            var attack_pos = GetAttackPos();
            var center = new Vector3((attack_pos.Item1.x + attack_pos.Item2.x)/2, (attack_pos.Item1.y + attack_pos.Item2.y)/2);
            Gizmos.DrawWireCube(center, new Vector3(range.x, range.y));
        }
    }
}
