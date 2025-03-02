using UnityEngine;
using System.Collections;

public class MoveP2 : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float friction = 0.5f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    private bool isDashing = false;
    private bool isGrounded;
    private bool canAirDash = true;
    private bool canDash = true;

    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }

        if (!isDashing)
        {
            if (moveInput != 0)
            {
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, moveInput * speed, Time.deltaTime * acceleration), rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x * friction, rb.linearVelocity.y);
            }
        }

        if (moveInput < 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput > 0 && facingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Keypad3) && canDash && (isGrounded || canAirDash))
        {
            StartCoroutine(Dash());
            if (!isGrounded)
            {
                canAirDash = false;
            }
        }
    }


    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2((facingRight ? -1 : 1) * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(2f); // Thời gian hồi chiêu lướt
        canDash = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canAirDash = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
