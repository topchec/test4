using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class animator : MonoBehaviour
{
    private bool WallSlide;
    private Rigidbody2D rb;
    private float AnimState;
    private Animator a;
    private float AirSpeedY;
    private SpriteRenderer sp;


    [Header("Ground Detection")]
    public float groundCheckRadius = 0.2f;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;

    private float horizontalInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canWallJump = true;
    private float wallJumpTimer;

    [Header("Wall Detection")]
    public float wallCheckDistance = 0.6f;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       CheckCollisions();
        anim();
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

        
    }
    void anim()
    {
        if (isTouchingWall)
        {
            WallSlide = false;
        }
        else { WallSlide = true; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            a.SetTrigger("Jump");
        }
        AirSpeedY = rb.velocity.y;


    }
}
