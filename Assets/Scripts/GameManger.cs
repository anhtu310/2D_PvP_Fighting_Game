using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Cập nhật hình nền map
        SpriteRenderer spriteRenderer = map.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = maps[mapIndex];

        // Lật nhân vật 2 để đối diện nhân vật 1
        Vector3 scale = player2.transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        player2.transform.localScale = scale;

        // Thêm PlayerController vào từng nhân vật
        PlayerController p1Controller = player1.AddComponent<PlayerController>();
        PlayerController p2Controller = player2.AddComponent<PlayerController>();

        // Đánh dấu Player 1 và Player 2
        p1Controller.isPlayer1 = true;
        p2Controller.isPlayer1 = false;

        // Gán 2 nhân vật vào CameraManager để camera theo dõi
        CameraManage cameraManager = FindFirstObjectByType<CameraManage>();
        if (cameraManager != null)
        {
            cameraManager.SetPlayers(player1.transform, player2.transform);
        }
    }

    
}

