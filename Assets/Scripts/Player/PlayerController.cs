using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;

    [Header("Ground Check Settings")]
    public Transform groundCheckPoint; // �����, ������ ����� ����������� ���
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Audio Settings")]
    public AudioSource audioSource; // �������� �����

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float horizontalInput;
    private PlayerHealth playerHealth;
    private bool isDead = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isRunningSoundPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnDeath += HandleDeath;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        // ����������� ������
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // ������� �������
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;

        // ���������� ����������
        animator.SetBool("isWalking", horizontalInput != 0);
        animator.SetBool("isRunning", isRunning && horizontalInput != 0);
        animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0);
        animator.SetBool("isFalling", !isGrounded && rb.velocity.y < 0);

        // �������� ���������� �� �����
        CheckGroundStatus();

        // ����� ������������
        if (isGrounded && horizontalInput != 0 && !isRunningSoundPlaying)
        {
            string soundKey = isRunning ? "run" : "step";
            PlayFootstepSound(soundKey);
        }

        // ������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // ����� ������������ �������� ����� �������
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // ��������������� ����� ������
        SoundManager.Instance.PlaySound("jump", audioSource);
    }

    private void CheckGroundStatus()
    {
        // ������ ���� ���� ��� �������� �� �����
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    private void PlayFootstepSound(string soundKey)
    {
        SoundManager.Instance.PlaySound(soundKey, audioSource);
        StartCoroutine(FootstepSoundCooldown());
    }

    private IEnumerator FootstepSoundCooldown()
    {
        isRunningSoundPlaying = true;
        yield return new WaitForSeconds(0.3f); // ����� �� ���������� ����� ����/���� (����������� �� �������������)
        isRunningSoundPlaying = false;
    }

    private void HandleDeath()
    {
        isDead = true;
        animator.SetTrigger("Death");
        SoundManager.Instance.PlaySound("death", audioSource); // ��������������� ����� ������
        // ��������� ����������
        enabled = false;
        // ����� �������� ����� ����������� ������ ����� ������������ �����
        StartCoroutine(RestartLevelAfterDelay());
    }

    private IEnumerator RestartLevelAfterDelay()
    {
        yield return new WaitForSeconds(2f); // ���� ��������� �������� ������
        // ����� �������� ������ ����������� ������
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandleDeath;
        }
    }

    // ������������ ������� �������� � ��������� (�����������)
    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
