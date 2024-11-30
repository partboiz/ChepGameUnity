using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public int speed = 6;
    public float jumpForce = 7f;
    private float InputHorizontal;
    private Animator anim;
    private bool canJump = true;
    private int jumpCount = 0;
    private const int MAX_JUMPS = 2;
    public float moveThreshold = 0.1f;
    public Text Score;
    public float count;
    public GameObject text, canvas;
    public int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject kunaiPrefab;

    private enum MomentStase { idle, run, jump, fall }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        InputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(InputHorizontal * speed, rb.velocity.y);
        Animation();
        Jump();
        Attack();
        KunaiAttack();
    }

    public void Animation()
    {
        MomentStase stase;
        if (InputHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            stase = MomentStase.run;
        }
        else if (InputHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            stase = MomentStase.run;
        }
        else
        {
            stase = MomentStase.idle;
        }
        if (rb.velocity.y > 0.1)
        {
            stase = MomentStase.jump;
        }
        else if (rb.velocity.y < -0.1)
        {
            stase = MomentStase.fall;
        }
        anim.SetInteger("Stase", (int)stase);
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && jumpCount < MAX_JUMPS)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            if (jumpCount >= MAX_JUMPS)
            {
                canJump = false;
            }
        }
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && !IsMoving())
        {
            Attacking();
            TakeDamage(20);
        }
    }

    public void KunaiAttack()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
            if (kunaiPrefab != null && throwPoint != null)
            {
                GameObject kunai = ObjectPool.Instance.GetPooledObject();
                if (kunai != null)
                {
                    kunai.transform.position = throwPoint.position;
                    kunai.transform.rotation = throwPoint.rotation;
                    kunai.SetActive(true);

                    // Xác định hướng bắn dựa trên hướng di chuyển của người chơi
                    Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

                    // Áp dụng lực cho kunai
                    Rigidbody2D kunaiRb = kunai.GetComponent<Rigidbody2D>();
                    if (kunaiRb != null)
                    {
                        kunaiRb.velocity = shootDirection * 10f; // Bạn có thể điều chỉnh tốc độ ở đây
                    }
                }
            }
            else
            {
                Debug.LogWarning("Kunai prefab or throw point is missing!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Implement death logic here
        Debug.Log("Player has died!");
        // You might want to disable the player, show a game over screen, etc.
    }

    bool IsMoving()
    {
        return rb.velocity.magnitude > moveThreshold;
    }

    void Attacking()
    {
        anim.SetTrigger("Attack");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Coin"))
        {
            var name = collider2D.attachedRigidbody.name;
            Destroy(GameObject.Find(name));
            count++;
            Score.text = "Score: " + count;
        }
    }
}