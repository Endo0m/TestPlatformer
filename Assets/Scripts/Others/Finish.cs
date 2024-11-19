using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private string finishSoundKey; // ���� ����� ��������� ������
    private AudioSource audioSource;
    private bool hasTriggered = false; // ���� ��� �������������� ���������� ������������

    private void Start()
    {
        // �������� ��� ��������� ��������� AudioSource
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
            hasTriggered = true; // ������������� ����, ����� ������������� ��������� ������������
            StartCoroutine(PlaySoundAndLoadScene());
        }
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        // ������������� ���� ��������� ������ ����� SoundManager
        if (!string.IsNullOrEmpty(finishSoundKey))
        {
            SoundManager.Instance.PlaySound(finishSoundKey, audioSource);
        }

        // ���� ��������� ��������������� �����
        if (audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // ������� �� ����� ����
        SceneManager.LoadScene("Menu");
    }
}
