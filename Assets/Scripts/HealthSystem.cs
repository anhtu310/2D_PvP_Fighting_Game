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
        if (character.IsInvincible) return;

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
        DisableAllComponentsExceptAnimator(gameObject);

        string opponentTag = CompareTag("Player1") ? "Player2" : "Player1";
        GameObject opponent = GameObject.FindGameObjectWithTag(opponentTag);

        if (opponent != null)
        {
            Animator opponentAnimator = opponent.GetComponent<Animator>();
            if (opponentAnimator != null)
            {
                opponentAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }

            DisableAllComponentsExceptAnimator(opponent);
        }
    }

    void DisableAllComponentsExceptAnimator(GameObject obj)
    {
        Component[] components = obj.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component is Animator) continue; // Giữ lại Animator
            if (component is MonoBehaviour script) script.enabled = false; // Vô hiệu hóa script
        }

        obj.SetActive(true); // Đảm bảo GameObject vẫn hoạt động
    }

    IEnumerator ShowKOThenPause()
    {
        CameraManage cam = FindAnyObjectByType<CameraManage>();
        cam.showKO();

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 0;
    }

    IEnumerator FlashDamageEffect()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, character.maxHealth);
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, character.maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }
}
