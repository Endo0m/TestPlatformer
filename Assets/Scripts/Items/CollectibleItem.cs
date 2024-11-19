using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemKey; // ���� ��� ������������� ���� ��������
    public Animator animator; // ������ �� ��������� ��������

    private Collider2D itemCollider; // ������ �� ���������
    private bool isCollected = false; // ���� ��� ������������ ��������� �������

    [SerializeField] private float timeAnimation = 0.5f;
    private AudioSource audioSource;
    [SerializeField] private string soundKey;
    private void Awake()
    {
        // �������� ������ �� ���������
        itemCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ��� ����� ��������������� � ��������� � �� ��� �� ��� ������
        if (other.CompareTag("Player") && !isCollected)
        {
            SoundManager.Instance.PlaySound(soundKey, audioSource);
            isCollected = true; // ������������� ����, ����� ������������� ��������� ������
            itemCollider.enabled = false; // ��������� ���������, ����� �������� ���������� ��������������

            // ����������� �������� �������
            if (animator != null)
            {
                animator.SetTrigger("Collect"); 
            }

            // ��������� �������� ��� �������� �������� ����� ���������� ��������
            StartCoroutine(HandleItemCollection());
        }
    }

    private System.Collections.IEnumerator HandleItemCollection()
    {
        // ��� ���������� �������� (����� ����� ���� �������� ������������� ��������)
        yield return new WaitForSeconds(animator != null ? animator.GetCurrentAnimatorStateInfo(0).length : timeAnimation);

        // ��������� ������� � ���������
        ItemManager.Instance.AddItem(itemKey);

        // ���������� ������
        Destroy(gameObject);
    }
}
