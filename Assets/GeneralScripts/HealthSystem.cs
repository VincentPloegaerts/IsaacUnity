using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<float> OnLostHealth;
    public event Action<float> OnRegainHealth;
    public event Action<float, float> OnHealthChanged;
    [SerializeField] float currentHealth = 100.0f;
    [SerializeField] float maxHealth = 100.0f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public void TakeDamage(float _damage)
    {
        if (_damage <= 0) return;
        currentHealth -= _damage;
        currentHealth = currentHealth < 0 ? 0.0f : currentHealth;
        OnLostHealth?.Invoke(_damage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void RegainHealth(float _regen)
    {
        if (_regen <= 0) return;
        currentHealth += _regen;
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
        OnRegainHealth?.Invoke(_regen);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetMaxHealth(float _newMax,bool _regen = false)
    {
        maxHealth = _newMax;
        if (_regen)
            currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}