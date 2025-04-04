﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterPrefabs;
    [SerializeField] private Transform spawnPoint1, spawnPoint2, itemsPoint;
    [SerializeField] private List<Sprite> maps;
    [SerializeField] private GameObject map;
    [SerializeField] private List<Sprite> characterAvatars;
    [SerializeField] private Image avatarImage1, avatarImage2;
    [SerializeField] private List<GameObject> itemsPrefab;

    private float itemSpawnInterval = 10f; // Thời gian spawn item mỗi 5s

    void Start()
    {
        SpawnCharacters();
        StartCoroutine(SpawnItemsRoutine()); // Bắt đầu Coroutine spawn item
    }

    void SpawnCharacters()
    {
        int p1Index = PlayerPrefs.GetInt("Player1Index", 0);
        int p2Index = PlayerPrefs.GetInt("Player2Index", 0);
        int mapIndex = PlayerPrefs.GetInt("MapIndex", 0);
        avatarImage1.sprite = characterAvatars[p1Index];
        avatarImage2.sprite = characterAvatars[p2Index];
        avatarImage2.rectTransform.localScale = new Vector3(-1, 1, 1);

        GameObject player1 = InstantiateCharacter(characterPrefabs[p1Index], spawnPoint1, "Player1");
        GameObject player2 = InstantiateCharacter(characterPrefabs[p2Index], spawnPoint2, "Player2");

        FlipPlayer(player2);
        UpdateMap(mapIndex);

        AssignCameraTargets(player1.transform, player2.transform);

        player1.AddComponent<HealthSystem>();
        player2.AddComponent<HealthSystem>();
        player1.AddComponent<ManaSystem>();
        player2.AddComponent<ManaSystem>();
    }

    IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(itemSpawnInterval);
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        if (itemsPrefab.Count == 0) return;

        int randomIndex = Random.Range(0, itemsPrefab.Count);
        GameObject randomItem = itemsPrefab[randomIndex];

        InstantiateItems(randomItem, itemsPoint);
    }

    GameObject InstantiateItems(GameObject prefab, Transform spawnPoint)
    {
        Vector3 campos = Camera.main.transform.position;
        campos.z = 1;
        campos.x += 3;
        spawnPoint.transform.position = campos;
        GameObject item = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        return item;
    }

    GameObject InstantiateCharacter(GameObject prefab, Transform spawnPoint, string tag)
    {
        GameObject character = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        character.tag = tag;
        return character;
    }

    void FlipPlayer(GameObject player)
    {
        Vector3 scale = player.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        player.transform.localScale = scale;
    }

    void UpdateMap(int mapIndex)
    {
        SpriteRenderer renderer = map.GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.sprite = maps[mapIndex];
    }

    void AssignCameraTargets(Transform player1, Transform player2)
    {
        CameraManage cameraManager = FindFirstObjectByType<CameraManage>();
        if (cameraManager != null) cameraManager.SetPlayers(player1, player2);
        cameraManager.showFight();
    }
}
