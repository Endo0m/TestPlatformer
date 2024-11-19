using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int healthRestoreAmount = 1; // Количество восстанавливаемого здоровья
    [SerializeField] private string pickupSoundKey; // Ключ для звука подбора

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, есть ли у объекта компонент HealthSystem
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem != null && healthSystem.IsAlive)
        {
            // Восстанавливаем здоровье
            healthSystem.RestoreHealth(healthRestoreAmount);

            // Воспроизводим звук подбора (если SoundManager и ключ настроены)
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (!string.IsNullOrEmpty(pickupSoundKey) && audioSource != null)
            {
                SoundManager.Instance.PlaySound(pickupSoundKey, audioSource);
            }

            // Удаляем сердечко после подбора
            Destroy(gameObject);
        }
    }
}
