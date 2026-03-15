using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banditdeepseek : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] LayerMask groundLayer;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private bool m_grounded = true; // Временно всегда true
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
            Debug.LogError("No Animator found!");

        m_body2d = GetComponent<Rigidbody2D>();
        if (m_body2d == null)
            Debug.LogError("No Rigidbody2D found!");
    }

    void Update()
    {
        // ВРЕМЕННО: пропускаем grounded проверку
        m_animator.SetBool("Grounded", m_grounded);

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        if (m_body2d != null)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        if (m_body2d != null)
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!m_isDead && m_animator != null)
                m_animator.SetTrigger("Death");
            else if (m_animator != null)
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }

        //Hurt
        else if (Input.GetKeyDown("q") && m_animator != null)
            m_animator.SetTrigger("Hurt");

        //Attack
        else if (Input.GetMouseButtonDown(0) && m_animator != null)
            m_animator.SetTrigger("Attack");

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && m_body2d != null)
        {
            if (m_animator != null)
                m_animator.SetTrigger("Jump");

            m_grounded = false;

            if (m_animator != null)
                m_animator.SetBool("Grounded", m_grounded);

            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && m_animator != null)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle && m_animator != null)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else if (m_animator != null)
            m_animator.SetInteger("AnimState", 0);
    }

    // Простая проверка земли при столкновении
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_grounded = false;
        }
    }
}
