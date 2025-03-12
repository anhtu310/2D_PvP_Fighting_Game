using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private float damage;
    private Animator animator;
    private bool canExplode = true; // Mặc định có hiệu ứng nổ
    private Collider2D col;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (hit) return;
        transform.Translate(speed * Time.deltaTime * direction, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hit && ((gameObject.tag == "Projectile_P1" && collision.CompareTag("Player2")) ||
                     (gameObject.tag == "Projectile_P2" && collision.CompareTag("Player1"))))
        {
            hit = true;

            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Vô hiệu hóa collider để tránh va chạm nhiều lần
            if (col != null) col.enabled = false;

            if (canExplode && animator != null) // Kiểm tra có animation "Explode" không
            {
                animator.SetTrigger("Explode");

                // Hủy bất kỳ Invoke nào trước đó và hủy object sau 0.2s
                CancelInvoke("Deactivate");
                Invoke("Deactivate", 0.3f);
            }
            else
            {
                Deactivate(); // Nếu không có "Explode", hủy ngay
            }
        }
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        hit = false;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction), transform.localScale.y, transform.localScale.z);

        // Hủy viên đạn sau 1 giây nếu không chạm vào gì
        CancelInvoke("Deactivate");
        Invoke("Deactivate", 1f);
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public void SetCanExplode(bool value)
    {
        canExplode = value;
    }

    private void Deactivate()
    {
        Destroy(gameObject);
    }
}
