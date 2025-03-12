using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float currentHealth;
    private HealthBar healthBar;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CharacterBase character;

    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    void Start()
    {
        character = GetComponent<CharacterBase>();
        currentHealth = character.maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindAndAssignHealthBar();
    }

    void FindAndAssignHealthBar()
    {
        GameObject barObject = GameObject.Find(CompareTag("Player1") ? "HealthP1" : "HealthP2");

        if (barObject != null)
        {
            healthBar = barObject.GetComponent<HealthBar>();
            healthBar.SetHealth(currentHealth);
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
        StartCoroutine(FlashDamageEffect());

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

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, character.maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }
}
