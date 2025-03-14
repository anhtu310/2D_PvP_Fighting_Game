using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterPrefabs;
    [SerializeField] private Transform spawnPoint1, spawnPoint2;
    [SerializeField] private List<Sprite> maps;
    [SerializeField] private GameObject map;

    void Start() => SpawnCharacters();

    void SpawnCharacters()
    {
        int p1Index = PlayerPrefs.GetInt("Player1Index", 0);
        int p2Index = PlayerPrefs.GetInt("Player2Index", 0);
        int mapIndex = PlayerPrefs.GetInt("MapIndex", 0);

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
    }
}