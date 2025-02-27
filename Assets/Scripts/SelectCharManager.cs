using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using NUnit.Framework;
using System;
public class SelectCharManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr1;
    [SerializeField] private SpriteRenderer sr2;
    [SerializeField] private SpriteRenderer srMap;
    [SerializeField] private List<Sprite> characters = new List<Sprite>();
    [SerializeField] private List<Sprite> characters2 = new List<Sprite>();
    [SerializeField] private List<Sprite> maps = new List<Sprite>();
    private int selectedChar1 = 0;
    private int selectedChar2 = 0;
    private int selectedMap = 0;
    [SerializeField] private GameObject playerChar1;
    [SerializeField] private GameObject playerChar2;
    [SerializeField] private GameObject map;

    public void NextOption()
    {
        selectedChar1 = selectedChar1 + 1;
        if(selectedChar1 == characters.Count)
        {
            selectedChar1 = 0;
        }
        sr1.sprite = characters[selectedChar1];
    }

    public void NextOption2()
    {
        selectedChar2 = selectedChar2 + 1;
        if (selectedChar2 == characters2.Count)
        {
            selectedChar2 = 0;
        }
        sr2.sprite = characters2[selectedChar2];
    }

    public void NextMap()
    {
        selectedMap = selectedMap + 1;
        if (selectedMap == maps.Count)
        {
            selectedMap = 0;
        }
        srMap.sprite = maps[selectedMap];
    }

    public void BackOption()
    {
        selectedChar1 = selectedChar1 - 1;
        if(selectedChar1 < 0)
        {
            selectedChar1 = characters.Count - 1;
        }
        sr1.sprite = characters[selectedChar1];
    }

    public void BackOption2()
    {
        selectedChar2 = selectedChar2 - 1;
        if (selectedChar2 < 0)
        {
            selectedChar2 = characters2.Count - 1;
        }
        sr2.sprite = characters2[selectedChar2];
    }

    public void BackMap()
    {
        selectedMap = selectedMap - 1;
        if (selectedMap < 0)
        {
            selectedMap = maps.Count - 1;
        }
        srMap.sprite = maps[selectedMap];
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("Player1Index", selectedChar1);
        PlayerPrefs.SetInt("Player2Index", selectedChar2);
        PlayerPrefs.SetInt("MapIndex", selectedMap);
        PlayerPrefs.Save(); // Lưu dữ liệu vào bộ nhớ

        // In ra console để kiểm tra
        Debug.Log($"Player 1 chọn nhân vật index: {selectedChar1}");
        Debug.Log($"Player 2 chọn nhân vật index: {selectedChar2}");

        SceneManager.LoadScene("MainGame");
    }
}
