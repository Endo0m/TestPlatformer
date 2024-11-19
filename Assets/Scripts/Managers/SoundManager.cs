using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [System.Serializable]
    public struct Sound
    {
        public string key; // ��� �����
        public AudioClip clip; // ���������
    }

    public List<Sound> sounds; // ������ ���� ������

    // ������� ��� �������� ������ �� �����
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        // ���������� Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ��������� ������ ��� ����� ���� (�����������)
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // ������� ����������� ���������
            return;
        }

        // ������������� �������
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (Sound sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.key))
            {
                soundDictionary.Add(sound.key, sound.clip);
            }
            else
            {
                Debug.LogWarning($"���� � ������ '{sound.key}' ��� ���������� � �������.");
            }
        }
    }

    // ����� ��� ��������� ���������� �� �����
    public AudioClip GetAudioClip(string key)
    {
        if (soundDictionary.TryGetValue(key, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning($"��������� � ������ '{key}' �� ������.");
            return null;
        }
    }

    // ����� ��� ��������������� ���������� ����� ���������� AudioSource
    public void PlaySound(string key, AudioSource audioSource)
    {
        AudioClip clip = GetAudioClip(key);
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
