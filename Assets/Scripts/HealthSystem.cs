using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private HealthBar healthBar;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Color damageColor = Color.red; // Màu khi bị tấn công
    [SerializeField] private float flashDuration = 0.2f;   // Thời gian hiệu ứng nhấp nháy

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(WaitAndFindHealthBar());
    }

    IEnumerator WaitAndFindHealthBar()
    {
        yield return new WaitForSeconds(1f);
        FindAndAssignHealthBar();
    }

    void FindAndAssignHealthBar()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas không được tìm thấy!");
            return;
        }

        GameObject barObject = GameObject.Find(CompareTag("Player1") ? "HealthP1" : "HealthP2");

        if (barObject != null)
        {
            healthBar = barObject.GetComponent<HealthBar>();
        }
        else
        {
            Debug.LogError("Không tìm thấy HealthP1 hoặc HealthP2!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        animator.SetTrigger("Hurt");

        StartCoroutine(FlashDamageEffect()); // Gọi hiệu ứng nhấp nháy

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetTrigger("Die");
    }

    IEnumerator FlashDamageEffect()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }
}
