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

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        // Tính toán vị trí trung tâm giữa hai nhân vật
        Vector3 middlePoint = (player1.position + player2.position) / 2f;

        // Giới hạn camera trong khoảng minX và maxX
        float clampedX = Mathf.Clamp(middlePoint.x, minX, maxX);

        // Cập nhật vị trí camera
        Vector3 targetPosition = new Vector3(clampedX, fixedY, defaultZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    // Phương thức để gán nhân vật từ GameManager
    public void SetPlayers(Transform p1, Transform p2)
    {
        player1 = p1;
        player2 = p2;
    }
}
