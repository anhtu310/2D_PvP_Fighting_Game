using System.Collections;
using UnityEngine;

public class CameraManage : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 5f; // Tốc độ di chuyển camera
    [SerializeField] private float minX = -5f; // Giới hạn trái
    [SerializeField] private float maxX = 5f; // Giới hạn phải
    [SerializeField] private float fixedY = 0f; // Giữ nguyên chiều cao camera
    [SerializeField] private float defaultZ = -10f; // Đảm bảo camera giữ khoảng cách
    [SerializeField] private GameObject ko;
    [SerializeField] private GameObject fight;
	public AudioSource fightSound;
	public AudioSource koSound;

	void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // Tính toán vị trí trung tâm giữa hai nhân vật
        Vector3 middlePoint = (player1.position + player2.position) / 2f;

        // Giới hạn camera trong khoảng minX và maxX
        float clampedX = Mathf.Clamp(middlePoint.x, minX, maxX);
        
       Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0;
        fight.transform.position = camPos;
        ko.transform.localPosition = camPos;

        // Cập nhật vị trí camera
        Vector3 targetPosition = new Vector3(clampedX, fixedY, defaultZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    public void showFight()
    {
        StartCoroutine(DelayShowFight());
        PlaySoundFight();
        
    }
    IEnumerator DelayShowFight()
    {
        yield return new WaitForSecondsRealtime(1f);
        fight.SetActive(true);
        fight.transform.localScale = Vector3.one * 3.5f;
        StartCoroutine(ScaleDownFight());
	}

    IEnumerator ScaleDownFight()
    {
        Vector3 targetScale = Vector3.one; // scale mặc định là (1,1,1)
        Vector3 startScale = fight.transform.localScale;
        float duration = 1.5f; // thời gian thu nhỏ về
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // không bị ảnh hưởng bởi Time.timeScale
            fight.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        fight.transform.localScale = targetScale;
        fight.SetActive(false);
    }
    public void showKO()
    {
        ko.SetActive(true);
        ko.transform.localScale = Vector3.one * 3f; // Scale lên x2 trước
        StartCoroutine(ScaleDownKO());
        PlaySoundKO();
    }

    IEnumerator ScaleDownKO()
    {
        Vector3 targetScale = Vector3.one; // scale mặc định là (1,1,1)
        Vector3 startScale = ko.transform.localScale;
        float duration = 1f; // thời gian thu nhỏ về
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // không bị ảnh hưởng bởi Time.timeScale
            ko.transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        ko.transform.localScale = targetScale;
    }

    // Phương thức để gán nhân vật từ GameManager
    public void SetPlayers(Transform p1, Transform p2)
    {
        player1 = p1;
        player2 = p2;
    }
	public void PlaySoundFight()
	{
		if (fightSound != null) fightSound.Play();
	}

	public void PlaySoundKO()
	{
		if (koSound != null) koSound.Play();
	}
}
