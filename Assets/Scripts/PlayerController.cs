using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;

    private bool jumpPressed;
    private bool isGrounded;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 14f;


    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private bool facingRight = true;
    private int faceDirection = 1;

    private void Awake()
    {
        CheckGround();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
    }

    void Update()
    {
        CheckGround();

        HandleAnimations();
        HandleFlip();
    }
    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;

    }

    public void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (!jumpPressed) return;
        if (!isGrounded) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpPressed = false;
        isGrounded = false;

        Debug.Log("Jumped");
    }

    private void HandleFlip()
    {
        if (rb.linearVelocity.x < 0 && facingRight || rb.linearVelocity.x > 0 && !facingRight)
            Flip();
    }

    private void Flip()
    {
        faceDirection *= -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
