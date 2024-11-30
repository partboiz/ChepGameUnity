using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private float _maxHealth = 100;
    private float _currentHealth;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private GameController _gameController;
    [SerializeField] private int _damageAmount;
    [SerializeField] private Transform _healBarTransform;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBarDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            TakeDamage(_damageAmount);
        }
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateHealthBar();
        if (_currentHealth <= 0)
        {
            _gameController.Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (_healthBarFill != null)
        {
            _healthBarFill.fillAmount = _currentHealth / _maxHealth;
        }
    }

    private void UpdateHealthBarDirection()
    {
        if (_healBarTransform != null)
        {
            // Lấy scale của parent (nhân vật)
            Vector3 parentScale = transform.localScale;

            // Đặt lại scale cho thanh máu, giữ nguyên hướng
            _healBarTransform.localScale = new Vector3(
                Mathf.Abs(_healBarTransform.localScale.x) * Mathf.Sign(parentScale.x),
                _healBarTransform.localScale.y,
                _healBarTransform.localScale.z
            );
        }
    }
}