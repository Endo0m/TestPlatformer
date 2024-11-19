using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemKey; // Ключ для идентификации типа предмета
    public Animator animator; // Ссылка на компонент анимации

    private Collider2D itemCollider; // Ссылка на коллайдер
    private bool isCollected = false; // Флаг для отслеживания состояния подбора

    [SerializeField] private float timeAnimation = 0.5f;
    private AudioSource audioSource;
    [SerializeField] private string soundKey;
    private void Awake()
    {
        // Получаем ссылку на коллайдер
        itemCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что игрок взаимодействует с предметом и он ещё не был собран
        if (other.CompareTag("Player") && !isCollected)
        {
            SoundManager.Instance.PlaySound(soundKey, audioSource);
            isCollected = true; // Устанавливаем флаг, чтобы предотвратить повторный подбор
            itemCollider.enabled = false; // Отключаем коллайдер, чтобы избежать повторного взаимодействия

            // Проигрываем анимацию подбора
            if (animator != null)
            {
                animator.SetTrigger("Collect"); 
            }

            // Запускаем корутину для удаления предмета после завершения анимации
            StartCoroutine(HandleItemCollection());
        }
    }

    private System.Collections.IEnumerator HandleItemCollection()
    {
        // Ждём завершения анимации (время может быть заменено длительностью анимации)
        yield return new WaitForSeconds(animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : timeAnimation);

        // Добавляем предмет в инвентарь
        ItemManager.Instance.AddItem(itemKey);

        // Уничтожаем объект
        Destroy(gameObject);
    }
}
