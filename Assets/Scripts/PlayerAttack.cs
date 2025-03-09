using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public KeyCode attackKey;
    public KeyCode skill1Key;
    public KeyCode skill2Key;

    private Animator animator;
    private float attackComboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement.isPlayer1)
        {
            attackKey = KeyCode.J;
            skill1Key = KeyCode.K;
            skill2Key = KeyCode.L;
        }
        else
        {
            attackKey = KeyCode.Alpha1;
            skill1Key = KeyCode.Alpha2;
            skill2Key = KeyCode.Alpha3;
        }
    }

    void Update()
    {
        HandleAttack();
        HandleSkills();
        CheckComboReset();
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey))
        {
            lastAttackTime = Time.time;
            attackComboStep++;
            if (attackComboStep > 2) attackComboStep = 1;

            animator.SetFloat("AttackType", attackComboStep);
            animator.SetTrigger("Attack");
        }
    }

    void CheckComboReset()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackComboStep = 0;
            animator.SetFloat("AttackType", 0);
        }
    }

    void HandleSkills()
    {
        if (Input.GetKeyDown(skill1Key)) animator.SetTrigger("Skill1");
        else if (Input.GetKeyDown(skill2Key)) animator.SetTrigger("Skill2");
    }
}
