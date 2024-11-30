using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    private HealthController playerHealth;
    public HealthBarBehaviour Healthbar;
    public float right, left;
    public int speed = 5;
    public bool isright;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        Healthbar.SetHealth(currentHealth, maxHealth);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            isAttacking = true;
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("meleeAttack");
            }
        }
        else
        {
            isAttacking = false;
        }

        if (!isAttacking)
        {
            Move();
        }
        else
        {
            // Dừng lại khi đang tấn công
            animator.SetBool("moving", false);
        }
    }

    private void Move()
    {
        var nam = transform.position.x;
        if (nam < left)
        {
            isright = true;
        }
        else if (nam > right)
        {
            isright = false;
        }

        animator.SetBool("moving", true);
        if (isright)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.Translate(new Vector3(Time.deltaTime * speed, 0, 0));
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Translate(new Vector3(-Time.deltaTime * speed, 0, 0));
        }

        if (nam <= left || nam >= right)
        {
            animator.SetBool("moving", true);
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
             0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<HealthController>();
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Healthbar.SetHealth(currentHealth, maxHealth);
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}