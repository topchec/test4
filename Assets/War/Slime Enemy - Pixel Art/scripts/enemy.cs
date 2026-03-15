using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float waitTime = 1f;

    private Vector2 startPosition;
    private Vector2 leftPoint;
    private Vector2 rightPoint;
    private bool movingRight = true;
    private bool isWaiting = false;
    private float waitTimer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Рассчитываем точки патрулирования
        leftPoint = startPosition + Vector2.left * patrolDistance;
        rightPoint = startPosition + Vector2.right * patrolDistance;
    }

    void Update()
    {
        if (isWaiting)
        {
            WaitAtPoint();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Определяем целевую позицию
        Vector2 targetPosition = movingRight ? rightPoint : leftPoint;

        // Двигаемся к целевой позиции
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Проверяем, достигли ли точки разворота
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.1f)
        {
            StartWaiting();
        }
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        rb.velocity = Vector2.zero;
    }

    void WaitAtPoint()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTime)
        {
            isWaiting = false;
            movingRight = !movingRight; // Меняем направление
        }
    }

    // Визуализация точек патрулирования в редакторе
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftPoint, 0.2f);
            Gizmos.DrawWireSphere(rightPoint, 0.2f);
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector2 currentPos = transform.position;
            Gizmos.DrawWireSphere(currentPos + Vector2.left * patrolDistance, 0.2f);
            Gizmos.DrawWireSphere(currentPos + Vector2.right * patrolDistance, 0.2f);
            Gizmos.DrawLine(currentPos + Vector2.left * patrolDistance, currentPos + Vector2.right * patrolDistance);
        }
    }
}