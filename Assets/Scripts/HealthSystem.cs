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
        if (character.IsInvincible) return; // Nếu bất tử, bỏ qua sát thương

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
        StartCoroutine(ShowKOThenPause());
    }

    IEnumerator ShowKOThenPause()
    {
        CameraManage cam = FindAnyObjectByType<CameraManage>();
        cam.showKO();

        yield return new WaitForSecondsRealtime(2f); // chờ 1s mà không bị ảnh hưởng bởi Time.timeScale

        Time.timeScale = 0;
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
