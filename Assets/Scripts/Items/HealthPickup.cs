using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthRestoreAmount = 1; // ���������� ������������������ ��������
    [SerializeField] private string pickupSoundKey; // ���� ��� ����� �������

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ���� �� � ������� ��������� HealthSystem
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null && healthSystem.IsAlive)
        {
            // ��������������� ��������
            healthSystem.RestoreHealth(healthRestoreAmount);

            // ������������� ���� ������� (���� SoundManager � ���� ���������)
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (!string.IsNullOrEmpty(pickupSoundKey) && audioSource != null)
            {
                SoundManager.Instance.PlaySound(pickupSoundKey, audioSource);
            }

            // ������� �������� ����� �������
            Destroy(gameObject);
        }
    }
}
