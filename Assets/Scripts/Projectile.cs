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
        if (!hit && (gameObject.tag == "Projectile_P1" && collision.CompareTag("Player2")) ||
                    (gameObject.tag == "Projectile_P2" && collision.CompareTag("Player1")))
        {
            hit = true;
            animator.SetTrigger("Explode");

            Animator playerAnimator = collision.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Hurt");
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

        Invoke("Deactivate", 2f);
    }

    private void Deactivate()
    {
        Destroy(gameObject);
    }
}
