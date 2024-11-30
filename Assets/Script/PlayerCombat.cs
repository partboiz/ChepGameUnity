using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;


    public float attackRanged = 0.5f;
    public float moveThreshold = 0.1f;
    public LayerMask enemyLayers;
    private Rigidbody2D rb;

    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Time.time >= nextAttackTime) 
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        
    }


    void Attack()
    {
        animator.SetTrigger("Attack");

       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRanged,enemyLayers);

        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawSphere(attackPoint.position, attackRanged);
    }
}