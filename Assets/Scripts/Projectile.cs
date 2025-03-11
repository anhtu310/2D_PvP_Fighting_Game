using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool hit;
    private float direction;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
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
                health.TakeDamage((int)10f); // Gây 10 sát thương
            }

            Invoke("Deactivate", 0.3f);
        }
    }


    public void SetDirection(float _direction)
    {
        direction = _direction;
        hit = false;

        float localScaleX = Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction);
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        Invoke("Deactivate", 1f);
    }

    private void Deactivate()
    {
        Destroy(gameObject);
    }
}
