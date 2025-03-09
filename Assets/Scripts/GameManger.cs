using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterPrefabs; // Danh sách Prefab nhân vật
    [SerializeField] private Transform spawnPoint1, spawnPoint2; // Vị trí để spawn nhân vật
    [SerializeField] private List<Sprite> maps = new List<Sprite>();
    [SerializeField] private GameObject map;

    void Start()
    {
        SpawnCharacter();
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

        // Thêm PlayerMovement và PlayerAttack vào từng nhân vật
        PlayerMovement p1Movement = player1.AddComponent<PlayerMovement>();
        PlayerAttack p1Attack = player1.AddComponent<PlayerAttack>();

        PlayerMovement p2Movement = player2.AddComponent<PlayerMovement>();
        PlayerAttack p2Attack = player2.AddComponent<PlayerAttack>();

        // Đánh dấu Player 1 và Player 2
        p1Movement.isPlayer1 = true;
        p2Movement.isPlayer1 = false;

        // Gán 2 nhân vật vào CameraManager để camera theo dõi
        CameraManage cameraManager = FindFirstObjectByType<CameraManage>();
        if (cameraManager != null)
        {
            cameraManager.SetPlayers(player1.transform, player2.transform);
        }
    }

    
}
