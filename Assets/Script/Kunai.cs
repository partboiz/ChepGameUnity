using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    private Rigidbody2D rb;
    public float throwForce = 10f;
    public float lifeTime = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterTime());
        Launch();
    }

    private void Launch()
    {
        rb.velocity = Vector2.zero; // Reset velocity
        rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Kunai collided with: " + collider.name);
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemyScript = collider.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                Debug.Log("Enemy found, applying damage.");
                enemyScript.TakeDamage(20);
            }
            gameObject.SetActive(false); // Deactivate instead of destroy
        }
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}