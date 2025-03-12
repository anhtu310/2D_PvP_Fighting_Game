using System.Collections;
using UnityEngine;

public class C2 : MonoBehaviour
{
    public KeyCode skill1Key = KeyCode.K;
    public KeyCode skill2Key = KeyCode.L;
    public bool isPlayer1 = false;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject C2Prefab;
    [SerializeField] private float projectileDamage = 15f;
    [SerializeField] private float healAmount = 30f;

    private Animator animator;
    private HealthSystem healthSystem;
    private bool isSkill1Queued;
    private bool isSkill2Queued;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        InitializeKeys();
    }

    void InitializeKeys()
    {
        if (CompareTag("Player1"))
        {
            isPlayer1 = true;
            skill1Key = KeyCode.K;
            skill2Key = KeyCode.L;
        }
        else if (CompareTag("Player2"))
        {
            isPlayer1 = false;
            skill1Key = KeyCode.Keypad2;
            skill2Key = KeyCode.Keypad3;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(skill1Key))
        {
            QueueSkill("Skill1", ref isSkill1Queued);
        }
        else if (Input.GetKeyDown(skill2Key))
        {
            QueueSkill("Skill2", ref isSkill2Queued);
        }
    }

    void QueueSkill(string skillTrigger, ref bool skillQueued)
    {
        animator.SetTrigger(skillTrigger);
        skillQueued = true;
    }

    // Gọi từ Animation Event
    public void SpawnSkill1()
    {
        if (!isSkill1Queued) return;
        isSkill1Queued = false;

        float direction = transform.localScale.x > 0 ? 1 : -1;
        GameObject projectile = Instantiate(C2Prefab, firePoint.position, Quaternion.identity);
        projectile.SetActive(true);
        projectile.tag = isPlayer1 ? "Projectile_P1" : "Projectile_P2";

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetDirection(direction);
            projScript.SetDamage(projectileDamage);
            projScript.SetCanExplode(false); // 🛠️ Tắt hiệu ứng nổ cho C2
        }
    }


    // Gọi từ Animation Event
    public void HealSkill2()
    {
        if (!isSkill2Queued) return;
        isSkill2Queued = false;

        if (healthSystem != null)
        {
            healthSystem.Heal(healAmount);
        }
    }
}
