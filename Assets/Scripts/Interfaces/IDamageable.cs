using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage, Vector2 hitPoint);
    bool IsAlive { get; }
    event System.Action<int> OnHealthChanged;
    event System.Action OnDeath;
}