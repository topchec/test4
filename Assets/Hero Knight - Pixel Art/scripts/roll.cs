using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roll : MonoBehaviour
{
    [Header("Roll Settings")]
    public float rollDistance = 3f;
    public float rollDuration = 0.3f;
    public float rollCooldown = 1.2f;
    public LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private bool isRolling = false;
    private bool canRoll = true;
    private Vector2 rollTargetPosition;
    private Vector2 rollStartPosition;
    private float rollStartTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll && !isRolling)
        {
            TryStartRoll();
        }

        if (isRolling)
        {
            UpdateRollMovement();
        }
    }

    void TryStartRoll()
    {
        // Определяем направление
        Vector2 direction = GetRollDirection();

        // Проверяем, нет ли препятствий на пути
        float checkDistance = rollDistance + 0.5f; // Небольшой запас
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, checkDistance, obstacleLayer);

        if (hit.collider == null)
        {
            StartRoll(direction);
        }
        // Если есть препятствие, катимся на меньшее расстояние
        else
        {
            float safeDistance = hit.distance - 0.3f; // Отступаем от препятствия
            if (safeDistance > 0.5f) // Минимальная дистанция переката
            {
                StartRoll(direction, safeDistance);
            }
        }
    }

    void StartRoll(Vector2 direction, float customDistance = -1f)
    {
        isRolling = true;
        canRoll = false;

        rollStartPosition = transform.position;
        float actualDistance = customDistance > 0 ? customDistance : rollDistance;
        rollTargetPosition = rollStartPosition + direction * actualDistance;
        rollStartTime = Time.time;

        // Делаем kinematic на время переката для точного позиционирования
        rb.isKinematic = true;
    }

    void UpdateRollMovement()
    {
        float timeSinceStart = Time.time - rollStartTime;
        float progress = timeSinceStart / rollDuration;

        if (progress < 1f)
        {
            // Плавное движение к целевой позиции
            Vector2 newPosition = Vector2.Lerp(rollStartPosition, rollTargetPosition, progress);
            rb.MovePosition(newPosition);
        }
        else
        {
            // Завершаем перекат
            rb.MovePosition(rollTargetPosition);
            EndRoll();
        }
    }

    Vector2 GetRollDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            return new Vector2(horizontal, vertical).normalized;
        }

        return transform.right;
    }

    void EndRoll()
    {
        isRolling = false;
        rb.isKinematic = false;
        Invoke(nameof(ResetRollCooldown), rollCooldown);
    }

    void ResetRollCooldown()
    {
        canRoll = true;
    }
}

