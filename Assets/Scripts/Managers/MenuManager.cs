using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemUIText
    {
        public string key;
        public TMP_Text text;
    }

    public ItemUIText[] itemUITextArray; 
    private void Start()
    {
        InitializeCollectedItemsUI();
    }

    public void InitializeCollectedItemsUI()
    {
        foreach (var itemUIText in itemUITextArray)
        {
            int totalCollected = PlayerPrefs.GetInt(itemUIText.key, 0);
            UpdateItemUI(itemUIText.key, totalCollected);
        }
    }

    private void UpdateItemUI(string key, int count)
    {
        foreach (var itemUIText in itemUITextArray)
        {
            if (itemUIText.key == key)
            {
                itemUIText.text.text = count.ToString();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        // ¬ редакторе Unity выход из игры через Application.Quit не будет работать.
        // —ледующа€ строка нужна только дл€ проверки в редакторе.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ¬ыход из игры
#endif
    }
}
