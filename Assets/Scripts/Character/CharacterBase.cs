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

    public GameObject transformPrefab; // Prefab mới khi dùng Skill3


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
    protected KeyCode skill3Key;

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
    private bool isTransformed = false;
    private float originalAttackDamage;

    private float lastLeftPressTime = -1f;
    private float lastRightPressTime = -1f;

    //Sound
	public AudioSource attackSound;
	public AudioSource jumpSound;
	public AudioSource dashSound;
	public AudioSource hitSound;
	public AudioSource skill1Sound;
	public AudioSource skill2Sound;
    public AudioSource transformSound;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Nếu không tìm thấy Animator trong object chính, tìm trong child objects
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError($"[{gameObject.name}] Animator is NULL! Kiểm tra lại Prefab của {gameObject.name}.");
            }
        }

        originalAttackDamage = attackDamage;
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
            skill3Key = KeyCode.U;
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
            skill3Key = KeyCode.Keypad4;
        }
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        HandleAttack();
        CheckComboReset();

        // Nếu đã biến hình thì không cho dùng skill
        if (isTransformed) return;

        // Kiểm tra nếu nhấn skill3 và mana đầy thì mới cho biến hình
        if (Input.GetKeyDown(skill3Key) && manaSystem.CurrentMana >= maxMana && transformPrefab != null)
        {
            TransformCharacter();
        }
    }


    private void TransformCharacter()
    {
        if (manaSystem.CurrentMana < maxMana || isTransformed) return; // Chỉ biến hình khi đủ mana và chưa biến hình
        if(transformSound != null) transformSound.Play();
        isTransformed = true; // Đánh dấu đã biến hình

        // Tăng gấp đôi chỉ số
        maxHealth *= (float)1.5;
        runSpeed *= 2;
        jumpForce *= (float) 1.2;
        attackDamage *= (float)1.2;

        // Cập nhật máu mới (không vượt quá maxHealth)
        float newHealth = Mathf.Min(healthSystem.GetCurrentHealth() * (float)1.5, maxHealth);
        healthSystem.SetHealth(newHealth);

        // Đổi hình dạng nhân vật (Sprite, Animator, v.v.)
        if (transformPrefab != null)
        {
            SpriteRenderer newSprite = transformPrefab.GetComponent<SpriteRenderer>();
            Animator newAnimator = transformPrefab.GetComponent<Animator>();

            if (newSprite != null) GetComponent<SpriteRenderer>().sprite = newSprite.sprite;
            if (newSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = newSprite.sprite;
                transform.localScale *= 1.5f; // Phóng to lên 1.5 lần
            }
            if (newAnimator != null)
            {
                RuntimeAnimatorController newAnimController = newAnimator.runtimeAnimatorController;
                if (newAnimController != null) animator.runtimeAnimatorController = newAnimController;
            }
        }

        // Trừ toàn bộ mana
        manaSystem.ChangeMana(-maxMana);
        manaSystem.ChangeMana(0);
        ManaBar mana = GetComponent<ManaBar>();
        mana.SetMaxMana(0);
        // Bắt đầu trừ máu dần theo thời gian
        StartCoroutine(DecreaseHealthOverTime(30f));
    }

    private IEnumerator DecreaseHealthOverTime(float amountPerSecond)
    {
        while (healthSystem.GetCurrentHealth() > 0)
        {
            healthSystem.TakeDamage(amountPerSecond);
            yield return new WaitForSeconds(1f);
        }

        // Khi máu về 0, bật trigger "Die"
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        yield return new WaitForSeconds(2f); // Chờ animation chết
        Destroy(gameObject); // Xóa nhân vật
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
			if (jumpSound != null) jumpSound.Play();
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

        //Collider2D col = GetComponent<Collider2D>();
        //col.enabled = false; // Tắt collider để không va chạm với đối thủ

        rb.linearVelocity = new Vector2((transform.localScale.x > 0 ? 1 : -1) * dashSpeed, 0);
		if (dashSound != null) dashSound.Play();
		yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
        //col.enabled = true; // Bật lại collider

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    int countAttack;
    private bool isBoostedDamage = false;

    private void HandleAttack()
    {
        if (Input.GetKeyDown(attackKey))
        {
            lastAttackTime = Time.time;
            countAttack = Mathf.Clamp(countAttack + 1, 0, 3); // Giới hạn tối đa là 3

            float finalDamage = attackDamage; // Mặc định lấy damage gốc
            if (countAttack == 3)
            {
                finalDamage *= 2; // X2 dame khi combo đạt 3
            }
            animator.SetFloat("AttackType", attackComboStep);
            animator.SetTrigger("Attack");

            StartCoroutine(ActivateAttackZone(attackDamage));
            attackComboStep++;
            if (attackComboStep > 3) attackComboStep = 0;
        }
    }

    private IEnumerator ActivateAttackZone(float damage)
    {
        if(isTransformed)
        {
            attackZone.transform.localScale = Vector3.one * 1.5f;
        }
        else
        {
            attackZone.SetActive(true);
        }

        DealDamageToEnemies(damage);

        yield return new WaitForSeconds(attackDuration);
        attackZone.SetActive(false);
    }


    private void CheckComboReset()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            countAttack = 0;
            attackComboStep = 0;
            animator.SetFloat("AttackType", 0);

            if (isBoostedDamage)
            {
                attackDamage = originalAttackDamage; // Reset lại damage
                isBoostedDamage = false;
            }
        }
    }


    private void DealDamageToEnemies(float damage)
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
                    if (!isTransformed)
                    {
                        Debug.Log($"Gây sát thương {damage} cho: {enemy.gameObject.name}");
                        enemyHealth.TakeDamage(damage);
                        if (hitSound != null) hitSound.Play();
                        manaSystem?.ChangeMana(5f);
                    }
                    else
                    {
                        Debug.Log($"Gây sát thương {damage} cho: {enemy.gameObject.name}");
                        enemyHealth.TakeDamage(damage);
                        healthSystem.Heal(20f);
                    }
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

    protected IEnumerator ActivateInvincibility(float duration)
    {
        IsInvincible = true; // Bật chế độ bất tử
        yield return new WaitForSeconds(duration);
        IsInvincible = false; // Tắt chế độ bất tử
    }

    public bool IsInvincible { get; private set; } = false;

    protected void QueueSkill(string skillName)
    {
        animator.SetTrigger(skillName);
    }

    protected bool CheckMana(int skillNumber, bool isAnimation)
    {
        float manaCost = (skillNumber == 1) ? manaCostSkill1 : manaCostSkill2;

        if (manaSystem.CurrentMana >= manaCost)
        {

            if(!isAnimation)
            {
                manaSystem.ChangeMana(-manaCost);
                rb.linearVelocity = Vector2.zero;
                return true;
            }else
            {
                return true;
            }
        }
        else
        {
            Debug.Log("Không đủ mana để dùng skill!");
            return false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator == null)
            {
                Debug.LogError("Animator is NULL! Kiểm tra xem Animator có được gán trong Start() không.");
            }
            else
            {
                animator.SetBool("IsGrounded", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Va chạm với: {collision.gameObject.name} (Tag: {collision.tag})");

        if (collision.CompareTag("Possion"))
        {
            healthSystem.TakeDamage(30);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Heal"))
        {
            HealSkill(30);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Speed"))
        {
            runSpeed += 3;
            Destroy(collision.gameObject);
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
	private void PlaySoundAttack()
	{
		if (attackSound != null) attackSound.Play();
	}


	private void PlaySoundSkill1()
    {
        if (skill1Sound != null) skill1Sound.Play();
    }

	private void PlaySoundSkill2()
	{
		if (skill2Sound != null) skill2Sound.Play();
	}

    private void PlaySoundSkill3()
    {
        if(transformSound != null) transformSound.Play();
    }
}
