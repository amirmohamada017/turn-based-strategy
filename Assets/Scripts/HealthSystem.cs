using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    
    private int _health = 100;
    
    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
