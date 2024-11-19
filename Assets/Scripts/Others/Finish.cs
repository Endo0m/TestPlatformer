using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private string finishSoundKey; // Ключ звука окончания уровня
    private AudioSource audioSource;
    private bool hasTriggered = false; // Флаг для предотвращения повторного срабатывания

    private void Start()
    {
        // Получаем или добавляем компонент AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Устанавливаем флаг, чтобы предотвратить повторное срабатывание
            StartCoroutine(PlaySoundAndLoadScene());
        }
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        // Воспроизводим звук окончания уровня через SoundManager
        if (!string.IsNullOrEmpty(finishSoundKey))
        {
            SoundManager.Instance.PlaySound(finishSoundKey, audioSource);
        }

        // Ждем окончания воспроизведения звука
        if (audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // Переход на сцену меню
        SceneManager.LoadScene("Menu");
    }
}
