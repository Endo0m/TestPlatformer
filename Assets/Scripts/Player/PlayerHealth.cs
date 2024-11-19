using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.InitializeHealthUI(maxHealth);
        // Подписываемся на изменение здоровья
        OnHealthChanged += UIManager.Instance.UpdateHealthUI;
    }

    protected void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        if (UIManager.Instance != null)
        {
            OnHealthChanged -= UIManager.Instance.UpdateHealthUI;
        }
    }
}