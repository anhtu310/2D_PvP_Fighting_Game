using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private float damage; // Biến lưu sát thương của viên đạn
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            animator.SetTrigger("Explode");

            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage); // Gây sát thương
            }

            Invoke("Deactivate", 0.3f);
        }
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        hit = false;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction), transform.localScale.y, transform.localScale.z);
        Invoke("Deactivate", 1f);
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void Deactivate()
    {
        Destroy(gameObject);
    }
}
