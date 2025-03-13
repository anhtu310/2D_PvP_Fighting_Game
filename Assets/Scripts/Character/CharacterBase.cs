using System.Collections;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 7f;
    public float doubleTapTime = 0.3f;
    public float dashSpeed = 20f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;

    public GameObject attackZone;
    public float attackDuration = 0.5f;
    public float attackDamage = 10f;
    public float manaCostSkill1;
    public float manaCostSkill2;

    public float maxHealth = 300f;
    public float maxMana = 150f;
    public float manaRegenRate = 2f;

    protected KeyCode leftKey;
    protected KeyCode rightKey;
    protected KeyCode jumpKey;
    protected KeyCode dashKey;
    protected KeyCode attackKey;
    protected KeyCode skill1Key;
    protected KeyCode skill2Key;

    private float attackComboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1f;

    protected Rigidbody2D rb;
    protected Animator animator;
    public HealthSystem healthSystem;
    public ManaSystem manaSystem;


    private bool isGrounded;
    private bool isRunning = false;
    private bool isDashing = false;
    private bool canDash = true;

    private float lastLeftPressTime = -1f;
    private float lastRightPressTime = -1f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        manaSystem = GetComponent<ManaSystem>();
        InitializeKeys();
    }

    private void InitializeKeys()
    {
        if (CompareTag("Player1"))
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey = KeyCode.W;
            dashKey = KeyCode.S;
            attackKey = KeyCode.J;
            skill1Key = KeyCode.K;
            skill2Key = KeyCode.L;
        }
        else if (CompareTag("Player2"))
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.UpArrow;
            dashKey = KeyCode.DownArrow;
            attackKey = KeyCode.Keypad1;
            skill1Key = KeyCode.Keypad2;
            skill2Key = KeyCode.Keypad3;
        }
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        HandleAttack();
        CheckComboReset();
    }

    private void HandleMovement()
    {
        if (isDashing) return;

        float moveDirection = 0;
        float speed = walkSpeed;

        if (Input.GetKeyDown(leftKey))
        {
            if (Time.time - lastLeftPressTime < doubleTapTime) isRunning = true;
            lastLeftPressTime = Time.time;
        }
        else if (Input.GetKeyDown(rightKey))
        {
            if (Time.time - lastRightPressTime < doubleTapTime) isRunning = true;
            lastRightPressTime = Time.time;
        }

        if (Input.GetKey(leftKey)) moveDirection = -1;
        else if (Input.GetKey(rightKey)) moveDirection = 1;
        else isRunning = false;

        if (isRunning) speed = runSpeed;

        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);

        if (moveDirection != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveDirection) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2((transform.localScale.x > 0 ? 1 : -1) * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey))
        {
            lastAttackTime = Time.time;
            attackComboStep++;
            if (attackComboStep > 2) attackComboStep = 1;

            animator.SetFloat("AttackType", attackComboStep);
            animator.SetTrigger("Attack");

            StartCoroutine(ActivateAttackZone());
        }
    }

    private IEnumerator ActivateAttackZone()
    {
        attackZone.SetActive(true);

        DealDamageToEnemies();

        yield return new WaitForSeconds(attackDuration);
        attackZone.SetActive(false);
    }

    private void CheckComboReset()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            attackComboStep = 0;
            animator.SetFloat("AttackType", 0);
        }
    }

    private void DealDamageToEnemies()
    {
        float attackRadius = attackZone.GetComponent<CircleCollider2D>().radius;
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackZone.transform.position, attackRadius);

        foreach (Collider2D enemy in enemiesHit)
        {
            if ((CompareTag("Player1") && enemy.CompareTag("Player2")) ||
                (CompareTag("Player2") && enemy.CompareTag("Player1")))
            {
                HealthSystem enemyHealth = enemy.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    Debug.Log($"Gây sát thương cho: {enemy.gameObject.name}");
                    enemyHealth.TakeDamage(attackDamage);

                    manaSystem?.ChangeMana(5f);
                }
            }
        }
    }


    protected void HealSkill(float healAmount)
    {
        if (healthSystem != null)
        {
            healthSystem.Heal(healAmount);
        }
    }

    protected IEnumerator FireProjectileWithChargeOption(GameObject projectilePrefab, Transform firePoint, float damage, bool canExplode, bool isCharging)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.SetActive(true);
        projectile.tag = CompareTag("Player1") ? "Projectile_P1" : "Projectile_P2";

        Animator projAnimator = projectile.GetComponent<Animator>();
        if (projAnimator != null) projAnimator.enabled = false;

        if (isCharging)
        {
            // Nếu đang charge, đợi một thời gian trước khi bắn
            yield return new WaitForSeconds(0.2f);
            if (projAnimator != null) projAnimator.enabled = true;
        }
        else
        {
            // Nếu không charge, bật animation ngay lập tức
            if (projAnimator != null) projAnimator.enabled = true;
        }

        float direction = transform.localScale.x > 0 ? 1 : -1;
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (projScript != null)
        {
            projScript.SetDirection(direction);
            projScript.SetDamage(damage);
            projScript.SetCanExplode(canExplode);
        }
    }


    protected void QueueSkill(string skillName)
    {
        animator.SetTrigger(skillName);
    }

    protected void TryUseSkill(int skillNumber, string skillTrigger)
    {
        float manaCost = (skillNumber == 1) ? manaCostSkill1 : manaCostSkill2;

        if (manaSystem.CurrentMana >= manaCost)
        {
            manaSystem.ChangeMana(-manaCost); // 🔥 Trừ mana
            QueueSkill(skillTrigger); // Gọi skill theo tên animation
        }
        else
        {
            Debug.Log("Không đủ mana để dùng skill!");
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }
}
