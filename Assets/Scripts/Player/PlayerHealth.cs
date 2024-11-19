using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.InitializeHealthUI(maxHealth);
        // ������������� �� ��������� ��������
        OnHealthChanged += UIManager.Instance.UpdateHealthUI;
    }

    protected void OnDestroy()
    {
        // ������������ ��� ����������� �������
        if (UIManager.Instance != null)
        {
            OnHealthChanged -= UIManager.Instance.UpdateHealthUI;
        }
    }
}