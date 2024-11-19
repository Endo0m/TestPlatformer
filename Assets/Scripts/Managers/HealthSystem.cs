using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 5;
    [Header("Knockback Settings")]
    [SerializeField] protected float knockbackForce = 10f;
    [SerializeField] protected float knockbackDuration = 0.25f;
    [SerializeField] protected float invulnerabilityDuration = 1f;

    protected int currentHealth;
    protected bool isInvulnerable;
    protected Rigidbody2D rb;
    protected bool isKnockedBack;
    protected float knockbackTimer;

    public bool IsAlive => currentHealth > 0;
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnDeath;
    private AudioSource audioSource;
    [SerializeField] private string soundKeyDamage;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    protected virtual void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                // Восстанавливаем контроль над персонажем
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }
    public virtual void RestoreHealth(int amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public virtual void TakeDamage(int damage, Vector2 hitPoint)
    {
        if (!IsAlive || isInvulnerable) return;
        SoundManager.Instance.PlaySound(soundKeyDamage, audioSource);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        ApplyKnockback(hitPoint);
        StartCoroutine(InvulnerabilityCoroutine());

        if (!IsAlive)
        {
            OnDeath?.Invoke();
        }
    }

    protected virtual void ApplyKnockback(Vector2 hitPoint)
    {
        if (rb == null) return;

        Vector2 knockbackDirection;

        // Определяем направление отбрасывания
        Vector2 hitDirection = (Vector2)transform.position - hitPoint;

        // Если удар пришел сверху (например, упали на шипы)
        if (hitDirection.y < 0 && Mathf.Abs(hitDirection.y) > Mathf.Abs(hitDirection.x))
        {
            knockbackDirection = Vector2.up; // Подбрасываем вверх
        }
        else
        {
            // Нормализуем направление и сохраняем только горизонтальную составляющую
            knockbackDirection = new Vector2(Mathf.Sign(hitDirection.x), 0.5f).normalized;
        }

        // Применяем силу
        rb.velocity = Vector2.zero; // Сбрасываем текущую скорость
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Устанавливаем таймер отбрасывания
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
    }

    protected IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        // Мигание спрайта для визуальной обратной связи
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float blinkTime = 0.1f;
            int blinkCount = Mathf.FloorToInt(invulnerabilityDuration / (blinkTime * 2));

            for (int i = 0; i < blinkCount; i++)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(blinkTime);
                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(blinkTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(invulnerabilityDuration);
        }

        isInvulnerable = false;
    }
}