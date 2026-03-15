using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2: MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckRadius = 0.2f;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 10;

    [Header("Components")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform attackPoint;

    // Компоненты
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Переменные состояния
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool canAttack = true;
    private bool isAttacking = false; // ДОБАВЛЕНО!
    private bool isJumping = false;
    private bool isRunning = false;

    // Animation parameters
    private readonly int animSpeed = Animator.StringToHash("Speed");
    private readonly int animJump = Animator.StringToHash("Jump");
    private readonly int animGrounded = Animator.StringToHash("Grounded");
    private readonly int animAttack = Animator.StringToHash("Attack");
    private readonly int animHurt = Animator.StringToHash("Hurt");
    private readonly int animIsAttacking = Animator.StringToHash("IsAttacking");

    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Создаем groundCheck если не установлен
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = gc.transform;
        }

        // Создаем attackPoint если не установлен
        if (attackPoint == null)
        {
            GameObject ap = new GameObject("AttackPoint");
            ap.transform.SetParent(transform);
            ap.transform.localPosition = new Vector3(0.5f, 0, 0);
            attackPoint = ap.transform;
        }
    }

    void Update()
    {
        GetInput();
        HandleAnimations();
        HandleAttackInput();
        HandleJumpInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    void HandleMovement()
    {
        // Движение по горизонтали
        float currentSpeed = isRunning ? moveSpeed * 1.5f : moveSpeed;
        float targetSpeed = horizontalInput * currentSpeed;

        // Плавное изменение скорости
        float currentXVelocity = rb.velocity.x;
        float newXVelocity = Mathf.Lerp(currentXVelocity, targetSpeed, Time.fixedDeltaTime * 10f);

        rb.velocity = new Vector2(newXVelocity, rb.velocity.y);

        // Поворот спрайта
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Корректируем позицию AttackPoint
        if (attackPoint != null)
        {
            Vector3 attackPos = attackPoint.localPosition;
            attackPos.x *= -1;
            attackPoint.localPosition = attackPos;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Сбрасываем прыжок при приземлении
        if (isGrounded && rb.velocity.y <= 0)
        {
            isJumping = false;
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Уменьшаем высоту прыжка при отпускании кнопки
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        animator.SetTrigger(animJump);
    }

    void HandleAttackInput()
    {
        if (Input.GetButtonDown("Fire1") && canAttack && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        canAttack = false;
        animator.SetBool(animIsAttacking, true); // Устанавливаем параметр анимации

        // Запускаем анимацию атаки
        animator.SetTrigger(animAttack);

        // Ждем перед проверкой попадания
        yield return new WaitForSeconds(0.2f);

        // Проверяем попадание
        CheckHit();

        // Ждем окончания атаки
        yield return new WaitForSeconds(attackCooldown - 0.2f);

        isAttacking = false;
        animator.SetBool(animIsAttacking, false); // Сбрасываем параметр анимации
        canAttack = true;
    }

    void CheckHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // Проверяем наличие скрипта EnemyHealth
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
                else
                {
                    // Если скрипта нет, просто уничтожаем
                    Debug.Log($"Hit enemy without EnemyHealth script: {enemy.name}");
                    Destroy(enemy.gameObject);
                }
            }
        }
    }

    void HandleAnimations()
    {
        // Скорость для анимации бега
        float speed = Mathf.Abs(rb.velocity.x);
        animator.SetFloat(animSpeed, speed);

        // Состояние на земле
        animator.SetBool(animGrounded, isGrounded);

        // Для анимации падения/прыжка
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
    }

    public void TakeDamage(int damage)
    {
        // Анимация получения урона
        animator.SetTrigger(animHurt);

        // Эффект отбрасывания
        float knockbackDirection = isFacingRight ? -1 : 1;
        rb.velocity = new Vector2(knockbackDirection * 3f, 5f);

        // Здесь можно добавить логику здоровья
        Debug.Log($"Player took {damage} damage");
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}

