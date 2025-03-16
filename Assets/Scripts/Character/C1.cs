using UnityEngine;
using System.Collections;

public class C1 : CharacterBase
{
    [SerializeField] private GameObject C1_1Prefab;
    [SerializeField] private Transform firePoint1;
    [SerializeField] private GameObject meleeSkill1Zone; // Vùng tấn công cận chiến

    [SerializeField] private float damageSkill1 = 15f;
    [SerializeField] private float meleeSkillDuration = 0.5f; // Thời gian hitbox tồn tại
    [SerializeField] private float meleeSkillDamage = 30f;

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(skill1Key)) TryUseSkill(1, "Skill1");
        if (Input.GetKeyDown(skill2Key)) TryUseMeleeSkill();
    }

    public void SpawnSkill1() => StartCoroutine(FireProjectileWithChargeOption(C1_1Prefab, firePoint1, damageSkill1, false, true));

    private void TryUseMeleeSkill()
    {
        Debug.Log($"[{gameObject.name}] Current Mana: {manaSystem.CurrentMana}, Mana Cost: {manaCostSkill2}");

        if (manaSystem.CurrentMana >= manaCostSkill2)
        {
            QueueSkill("Skill2");  // Gọi animation Skill2 
            manaSystem.ChangeMana(-manaCostSkill2);
            Debug.Log($"[{gameObject.name}] Đã sử dụng chiêu, mana còn lại: {manaSystem.CurrentMana}");
            StartCoroutine(ActivateMeleeSkill());
           
        }
        else
        {
            Debug.Log("Không đủ mana để dùng skill cận chiến!");
        }
    }


    public void SpawnMeleeSkill1() => StartCoroutine(ActivateMeleeSkill());

    private IEnumerator ActivateMeleeSkill()
    {
        if (meleeSkill1Zone == null)
        {
            Debug.LogError($"[{gameObject.name}] meleeSkill1Zone vẫn NULL khi dùng skill!");
            yield break;
        }

        Debug.Log($"[{gameObject.name}] Kích hoạt vùng tấn công cận chiến!");
        meleeSkill1Zone.SetActive(true);

        DealDamageInMeleeZone();

        yield return new WaitForSeconds(meleeSkillDuration);

        Debug.Log($"[{gameObject.name}] Tắt vùng tấn công cận chiến!");
        meleeSkill1Zone.SetActive(false);
    }


    private void DealDamageInMeleeZone()
    {
        float attackRadius = meleeSkill1Zone.GetComponent<CircleCollider2D>().radius;
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(meleeSkill1Zone.transform.position, attackRadius);

        foreach (Collider2D enemy in enemiesHit)
        {
            if ((CompareTag("Player1") && enemy.CompareTag("Player2")) ||
                (CompareTag("Player2") && enemy.CompareTag("Player1")))
            {
                HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    Debug.Log($"Gây sát thương cận chiến cho: {enemy.gameObject.name}");
                    enemyHealth.TakeDamage(meleeSkillDamage);
                }
            }
        }
    }
}
