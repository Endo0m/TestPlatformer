using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private Dictionary<string, int> itemCollection = new Dictionary<string, int>();

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

    public void AddItem(string key)
    {
        if (!itemCollection.ContainsKey(key))
        {
            itemCollection[key] = 0;
        }
        itemCollection[key]++;
        UIManager.Instance.UpdateItemUI(key, itemCollection[key]);

        // Сохранение в PlayerPrefs общего количества собранных предметов
        int totalCollected = PlayerPrefs.GetInt(key, 0) + 1;
        PlayerPrefs.SetInt(key, totalCollected);
        PlayerPrefs.Save(); // Сохранение изменений
    }


    public int GetTotalCollected(string key)
    {
        return itemCollection.ContainsKey(key) ? itemCollection[key] : 0;
    }
}
