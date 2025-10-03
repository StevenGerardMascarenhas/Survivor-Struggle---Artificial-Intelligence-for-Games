using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canNormalJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool knockback;
    private float knockbackStartTime;


    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpTimerSet = 0.15f;
    [SerializeField] private float variableJumpHeaightMultiplier = 0.5f;
    [SerializeField] private float airDragMultiplier = 0.95f;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float knockbackDuration;

    [SerializeField] private int amountOfJumps = 1;

    [SerializeField] private Vector2 knockbackSpeed;

    public float groundCheckRadius;
    
    public Transform groundCheck;
    
    public LayerMask whatIsGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckJump();
        CheckKnockback();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || amountOfJumpsLeft > 0)
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * variableJumpHeaightMultiplier);
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            anim.SetTrigger("jump");
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.linearVelocityY <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
           if (isGrounded)
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("run", movementInputDirection != 0);
        anim.SetBool("grounded", isGrounded);
    }

    private void ApplyMovement()
    {
        if (!isGrounded && movementInputDirection == 0 && !knockback)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX * airDragMultiplier, rb.linearVelocityY);
        }

        else if (canMove && !knockback)
        {
            rb.linearVelocity = new Vector2(movementSpeed * movementInputDirection, rb.linearVelocityY);
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.linearVelocity = new Vector2(knockbackSpeed.x * direction, 0f);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
