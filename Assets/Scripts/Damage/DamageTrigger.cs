using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool destroyOnContact = false;
    [SerializeField] private string targetTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Передаем точку столкновения
            damageable.TakeDamage(damage, transform.position);

            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }
}