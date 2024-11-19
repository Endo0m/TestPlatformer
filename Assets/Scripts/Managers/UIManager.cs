using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject[] healthIcons;
    [System.Serializable]
    public class ItemUIText
    {
        public string key;
        public TMP_Text text;
    }
    public ItemUIText[] itemUITextArray;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeHealthUI(int maxHealth)
    {
        if (healthIcons == null || healthIcons.Length == 0)
        {
            Debug.LogWarning("Health icons array is not set in UIManager!");
            return;
        }

        // ѕоказываем только нужное количество иконок
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                healthIcons[i].SetActive(i < maxHealth);
            }
            else
            {
                Debug.LogWarning($"Health icon at index {i} is null!");
            }
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        if (healthIcons == null || healthIcons.Length == 0)
        {
            Debug.LogWarning("Health icons array is not set in UIManager!");
            return;
        }

        // ќбновл€ем отображение иконок здоровь€
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                healthIcons[i].SetActive(i < currentHealth);
            }
        }
    }

    public void UpdateItemUI(string key, int count)
    {
        foreach (var itemUIText in itemUITextArray)
        {
            if (itemUIText.key == key)
            {
                itemUIText.text.text = count.ToString();
            }
        }
    }
}