using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterPrefabs; // Danh sách Prefab nhân vật
    [SerializeField] private Transform spawnPoint1, spawnPoint2; // Vị trí để spawn nhân vật
    [SerializeField] private List<Sprite> maps = new List<Sprite>();
    [SerializeField] private GameObject map;
    void Start()
    {
        SpawnCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCharacter()
    {
        int p1Index = PlayerPrefs.GetInt("Player1Index", 0);
        int p2Index = PlayerPrefs.GetInt("Player2Index", 0);
        int mapIndex = PlayerPrefs.GetInt("MapIndex", 0);

        GameObject player1 = Instantiate(characterPrefabs[p1Index], spawnPoint1.position, Quaternion.identity);
        GameObject player2 = Instantiate(characterPrefabs[p2Index], spawnPoint2.position, Quaternion.identity);
        player1.AddComponent<MoveP1>();
        player2.AddComponent<MoveP2>();

        SpriteRenderer spriteRenderer = map.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = maps[mapIndex];

        // Lật nhân vật 2 để quay vào nhân vật 1
        Vector3 scale = player2.transform.localScale;
        scale.x = -Mathf.Abs(scale.x); // Đảm bảo luôn là số âm
        player2.transform.localScale = scale;

        // Tìm CameraManager và gán 2 nhân vật
        CameraManage cameraManager = FindFirstObjectByType<CameraManage>();
        if (cameraManager != null)
        {
            cameraManager.SetPlayers(player1.transform, player2.transform);
        }
    }
}
