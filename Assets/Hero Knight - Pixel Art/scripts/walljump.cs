using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walljump : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public float wallJumpForce = 10f;
    public float wallJumpHorizontalForce = 8f;

    [Header("Wall Detection")]
    public float wallCheckDistance = 0.6f;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    [Header("Ground Detection")]
    public float groundCheckRadius = 0.2f;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 2f;
    public float wallJumpCooldown = 0.2f;
    

    // Компоненты
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canWallJump = true;
    private float wallJumpTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (wallCheckPoint == null) wallCheckPoint = transform;
        if (groundCheckPoint == null) groundCheckPoint = transform;
    }

    void Update()
    {
        GetInput();
        CheckCollisions();
        HandleWallSlide();
        HandleJump();
        HandleWallJumpCooldown();
    }

    void FixedUpdate()
    {
        if (!isWallSliding)
        {
            Move();
        }
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void CheckCollisions()
    {
        // Проверка земли
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // Проверка стены слева и справа
        RaycastHit2D leftWallHit = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.left,
            wallCheckDistance,
            wallLayer
        );

        RaycastHit2D rightWallHit = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.right,
            wallCheckDistance,
            wallLayer
        );

        isTouchingWall = leftWallHit.collider != null || rightWallHit.collider != null;

        // Если на земле - разрешаем прыжок от стены
        if (isGrounded)
        {
            canWallJump = true;
        }
    }

    void HandleWallSlide()
    {
        // Проверяем условия для скольжения по стене:
        // 1. Касаемся стены
        // 2. Не на земле
        // 3. Двигаемся в сторону стены (или стоим на месте)
        // 4. Падаем вниз
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
            

            // Ограничиваем скорость падения при скольжении по стене
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            isWallSliding = false;
            
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Обычный прыжок с земли
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            // Прыжок от стены
            else if (isWallSliding && canWallJump)
            {
                PerformWallJump();
            }
        }
    }

    void PerformWallJump()
    {
        // Определяем направление от стены
        RaycastHit2D leftWallHit = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.left,
            wallCheckDistance,
            wallLayer
        );

        RaycastHit2D rightWallHit = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.right,
            wallCheckDistance,
            wallLayer
        );

        float wallJumpDirection = 0f;

        // Если касаемся левой стены - прыгаем вправо
        if (leftWallHit.collider != null)
        {
            wallJumpDirection = 1f; // Вправо
        }
        // Если касаемся правой стены - прыгаем влево
        else if (rightWallHit.collider != null)
        {
            wallJumpDirection = -1f; // Влево
        }

        // Применяем силу прыжка от стены
        rb.velocity = new Vector2(wallJumpDirection * wallJumpHorizontalForce, wallJumpForce);

        // Временно запрещаем прыжок от стены
        canWallJump = false;
        wallJumpTimer = wallJumpCooldown;

        // Поворачиваем персонажа
        if (wallJumpDirection != 0)
        {
            FlipCharacter(wallJumpDirection > 0);
        }
    }

    void HandleWallJumpCooldown()
    {
        if (!canWallJump)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                canWallJump = true;
            }
        }
    }

    void Move()
    {
        float targetVelocityX = horizontalInput * moveSpeed;
        rb.velocity = new Vector2(targetVelocityX, rb.velocity.y);

        // Поворачиваем персонажа при движении
        if (horizontalInput > 0)
        {
            FlipCharacter(true);
        }
        else if (horizontalInput < 0)
        {
            FlipCharacter(false);
        }
    }

    void FlipCharacter(bool faceRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1 : -1);
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        // Визуализация проверки стены
        Gizmos.color = Color.blue;
        if (wallCheckPoint != null)
        {
            Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + Vector3.left * wallCheckDistance);
            Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + Vector3.right * wallCheckDistance);
        }

        // Визуализация проверки земли
        Gizmos.color = Color.green;
        if (groundCheckPoint != null)
        {
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}

