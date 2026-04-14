using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour
//{
//    public float m_speed = 3f;
//    public float m_jumpForce = 7.5f;
//    public float inputX = 1;
//    public float m_attackRange = 3f;
//    public float m_agroDistance = 5f;
//    public float m_jumpDistance = 5f;
//    public float m_attackArea;
//    public float m_attackCooldown = 3f;      // задержка между атаками

//    public int hp = 10;
//    public int damage = 2;
//    public Vector3 attackOffset;

//    private Animator m_animator;
//    private Rigidbody2D m_body2d;
//    public Sensor_Bandit m_groundSensor;
//    public Transform player;

//    public bool m_grounded = false;
//    public bool m_combatIdle = false;
//    private bool m_isDead = false;
//    public bool m_isJump = false;
//    public bool m_canJump = true;

//    public GameObject coin;

//    private float m_nextAttackTime = 0f;     // время следующей доступной атаки

//    void Start() 
//    {
//        m_animator = GetComponent<Animator>();
//        m_body2d = GetComponent<Rigidbody2D>();
//        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//    }

//    void Update() 
//    {
//        if (m_isDead) return; // не двигаемся и не атакуем после смерти

//        // Проверка касания земли
//        if (!m_grounded && m_groundSensor.State()) 
//        {
//            m_grounded = true;
//            m_animator.SetBool("Grounded", m_grounded);
//            m_isJump = false;
//        }

//        if (m_grounded && !m_groundSensor.State() && !m_isJump) 
//        {
//            m_grounded = false;
//            m_animator.SetBool("Grounded", m_grounded);
//        }
//        else if (m_grounded && !m_groundSensor.State() && m_isJump)
//        {
//            m_grounded = false;
//            m_animator.SetBool("Grounded", m_grounded);
//        }

//        // Обновляем направление к игроку (всегда, не только при прыжках)
//        UpdateDirection();

//        // Логика движения и атаки
//        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
//        if (distanceToPlayer <= m_attackRange)
//        {
//            // Остановиться и атаковать
//            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
//            m_animator.SetFloat("Speed", 0f);

//            if (Time.time >= m_nextAttackTime)
//            {
//                Attack();
//                m_nextAttackTime = Time.time + m_attackCooldown;
//            }
//        }
//        else if (distanceToPlayer <= m_agroDistance)
//        {
//            // Бежать к игроку
//            float move = inputX * m_speed;
//            m_body2d.velocity = new Vector2(move, m_body2d.velocity.y);
//            m_animator.SetFloat("Speed", Mathf.Abs(move));
//        }
//        else
//        {
//            // Стоять на месте (вне зоны агро)
//            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
//            m_animator.SetFloat("Speed", 0f);
//        }

//        // Ориентация спрайта
//        if (inputX > 0)
//            transform.localScale = new Vector3(-1.45f, 1.45f, 1.45f);
//        else if (inputX < 0)
//            transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);

//        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
//    }

//    private void UpdateDirection()
//    {
//        if (player != null)
//        {
//            if (player.position.x < transform.position.x)
//                inputX = -1;
//            else
//                inputX = 1;
//        }
//    }

//    public void StartCor()
//    {
//        StartCoroutine(JumpCoolDown());
//    }

//    public IEnumerator JumpCoolDown()
//    {
//        yield return new WaitForSeconds(10f);
//        m_canJump = true;
//    }

//    public void Attack()
//    {
//        // Запуск анимации атаки
//        m_animator.SetTrigger("Attack");

//        Vector3 pos = transform.position;
//        pos += transform.right * attackOffset.x * -inputX;
//        pos += transform.up * attackOffset.y;
//        Collider2D hitPlayer = Physics2D.OverlapCircle(pos, m_attackArea);
        
//        if (hitPlayer != null && hitPlayer.GetComponent<HeroKnight>())
//        {
//            hitPlayer.GetComponent<HeroKnight>().GetDamage(damage);
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        Vector3 pos = transform.position;
//        pos += transform.right * attackOffset.x * -inputX; 
//        pos += transform.up * attackOffset.y;
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(pos, m_attackArea);
//    }

//    public void GetDamage(int damage)
//    {
//        if (m_isDead) return;
//        hp -= damage;
//        m_animator.SetTrigger("Hurt");

//        if (hp <= 0)
//        {
//            m_isDead = true;
//            gameObject.tag = "Corsp";
//            m_animator.SetInteger("AnimState", 0);
//            m_animator.SetTrigger("Death");
//            // Отключаем движение
//            m_body2d.velocity = Vector2.zero;
//        }
//        else if (hp > 0)
//        {
//            m_animator.SetInteger("AnimState", 1);
//        }
//    }
//}
{
    public float m_speed = 3f;
    public float m_jumpForce = 7.5f;
    public float inputX = 1;
    public float m_attackRange = 3f;
    public float m_agroDistance = 5f;
    public float m_jumpDistance = 5f;
    public float m_attackArea;
    public float m_attackCooldown = 3f;      // задержка между атаками

    public int hp = 10;
    public int damage = 2;
    public Vector3 attackOffset;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    public Sensor_Bandit m_groundSensor;
    public Transform player;

    public bool m_grounded = false;
    public bool m_combatIdle = false;
    private bool m_isDead = false;
    public bool m_isJump = false;
    public bool m_canJump = true;

    public GameObject coin;

    private float m_nextAttackTime = 0f;     // время следующей доступной атаки

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (m_isDead) return; // не двигаемся и не атакуем после смерти

        // Проверка касания земли
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_isJump = false;
        }

        if (m_grounded && !m_groundSensor.State() && !m_isJump)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        else if (m_grounded && !m_groundSensor.State() && m_isJump)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Обновляем направление к игроку (всегда, не только при прыжках)
        UpdateDirection();

        // Логика движения и атаки
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= m_attackRange)
        {
            // Остановиться и атаковать
            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
            m_animator.SetFloat("Speed", 0f);

            if (Time.time >= m_nextAttackTime)
            {
                Attack();
                m_nextAttackTime = Time.time + m_attackCooldown;
            }
        }
        else if (distanceToPlayer <= m_agroDistance)
        {
            // Бежать к игроку
            float move = inputX * m_speed;
            m_body2d.velocity = new Vector2(move, m_body2d.velocity.y);
            m_animator.SetFloat("Speed", Mathf.Abs(move));
        }
        else
        {
            // Стоять на месте (вне зоны агро)
            m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
            m_animator.SetFloat("Speed", 0f);
        }

        // Ориентация спрайта
        if (inputX > 0)
            transform.localScale = new Vector3(-1.45f, 1.45f, 1.45f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);

        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
    }

    private void UpdateDirection()
    {
        if (player != null)
        {
            if (player.position.x < transform.position.x)
                inputX = -1;
            else
                inputX = 1;
        }
    }

    public void StartCor()
    {
        StartCoroutine(JumpCoolDown());
    }

    public IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(10f);
        m_canJump = true;
    }

    public void Attack()
    {
        // Запуск анимации атаки
        m_animator.SetTrigger("Attack");

        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x * -inputX;
        pos += transform.up * attackOffset.y;
        Collider2D hitPlayer = Physics2D.OverlapCircle(pos, m_attackArea);

        if (hitPlayer != null && hitPlayer.GetComponent<HeroKnight>())
        {
            hitPlayer.GetComponent<HeroKnight>().GetDamage(damage);
        }
    }

    // ========== ДОБАВЛЕННЫЙ МЕТОД ДЛЯ ANIMATION EVENT ==========
    void OnAttackEnd()
    {
        // Этот метод вызывается из анимации в конце атаки
        // Можно добавить любую логику, например:
        // - Сброс состояния атаки
        // - Возврат в Idle
        // - Разрешение следующей атаки

        // Если ничего не нужно делать, просто оставьте метод пустым
        // Debug.Log("Attack animation ended for Bandit");
    }
    // ============================================================

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x * -inputX;
        pos += transform.up * attackOffset.y;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, m_attackArea);
    }

    public void GetDamage(int damage)
    {
        if (m_isDead) return;
        hp -= damage;
        m_animator.SetTrigger("Hurt");

        if (hp <= 0)
        {
            m_isDead = true;
            gameObject.tag = "Corsp";
            m_animator.SetInteger("AnimState", 0);
            m_animator.SetTrigger("Death");
            // Отключаем движение
            m_body2d.velocity = Vector2.zero;
        }
        else if (hp > 0)
        {
            m_animator.SetInteger("AnimState", 1);
        }
    }
}