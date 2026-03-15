using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheckPoint;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void CheckGrounded()
    {
        // Проверяем, касаемся ли мы земли
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);

        // Визуализация в редакторе
        Debug.DrawRay(groundCheckPoint.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    void Jump()
    {
        // Сбрасываем вертикальную скорость для consistent прыжков
        rb.velocity = new Vector2(rb.velocity.x, 0);
        // Применяем силу прыжка
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
