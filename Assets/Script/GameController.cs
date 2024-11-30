using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private HealthController _playerHealthController;
    
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (_playerHealthController != null)
        {
            _playerHealthController.ResetHealth();
        }
        
        Respawn();
    }
    void Respawn()
    {
        transform.position = startPos;
    }
}
