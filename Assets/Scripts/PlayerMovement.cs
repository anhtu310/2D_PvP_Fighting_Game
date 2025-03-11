using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlayer1 = false;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 7f;
    public float doubleTapTime = 0.3f;
    public float dashSpeed = 20f;
    public float dashTime = 0.1f;
    public float dashCooldown = 0.5f;

    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode dashKey;
    public KeyCode attackKey;

    private float attackComboStep = 0;
    private float lastAttackTime = 0f;
    private float comboResetTime = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isRunning = false;
    private bool isDashing = false;
    private bool canDash = true;

    private float lastLeftPressTime = -1f;
    private float lastRightPressTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (isPlayer1)
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey = KeyCode.W;
            dashKey = KeyCode.S;
            attackKey = KeyCode.J;
        }
        else
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.UpArrow;
            dashKey = KeyCode.DownArrow;
            attackKey = KeyCode.Keypad1;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        HandleAttack();
        CheckComboReset();
    }

    void HandleMovement()
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

    void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
    }

    void HandleDash()
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }
}