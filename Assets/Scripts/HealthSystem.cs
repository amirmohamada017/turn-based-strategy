using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    
    private const int HealthMax = 100;
    private int _health;

    private void Awake()
    {
        _health = HealthMax;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        if (_health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)_health / HealthMax;
    }
}
