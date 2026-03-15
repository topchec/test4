using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 6.8f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private float               m_wallContactTime = 0f;
    private bool                m_wasWallSliding = false; // Для отслеживания перехода

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    void Update ()
    {
        m_timeSinceAttack += Time.deltaTime;

        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        bool isTouchingWallRight = (m_wallSensorR1.State() && m_wallSensorR2.State());
        bool isTouchingWallLeft = (m_wallSensorL1.State() && m_wallSensorL2.State());
        bool isTouchingWall = isTouchingWallRight || isTouchingWallLeft;
        
        // Сохраняем предыдущее состояние
        m_wasWallSliding = m_isWallSliding;
        
        // Определяем, движется ли персонаж от стены
        bool isMovingAwayFromWall = false;
        if (!m_grounded && isTouchingWall)
        {
            if (isTouchingWallRight)
                isMovingAwayFromWall = inputX < 0; // От правой стены движемся влево
            else if (isTouchingWallLeft)
                isMovingAwayFromWall = inputX > 0; // От левой стены движемся вправо
        }
        
        // Wall slide активен только если касаемся стены и НЕ движемся от нее
        if (!m_grounded && isTouchingWall && !isMovingAwayFromWall)
        {
            m_wallContactTime += Time.deltaTime;
            
            if (m_wallContactTime < 0.5f)
            {
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, 0f);
                m_isWallSliding = true;
            }
            else
            {
                m_isWallSliding = true;
                
                float slideTime = m_wallContactTime - 0.5f;
                float baseSlideSpeed = 1.7f;
                float maxSlideSpeed = 6.8f;
                float currentSlideSpeed = baseSlideSpeed;
                
                if (slideTime > 0)
                {
                    float acceleration = 3f * 1.7f;
                    currentSlideSpeed = baseSlideSpeed + Mathf.Min(slideTime * acceleration, maxSlideSpeed - baseSlideSpeed);
                }
                
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, -currentSlideSpeed);
                
                if (Time.frameCount % 18 == 0)
                    CreateWallSlideDust();
            }
        }
        else
        {
            m_isWallSliding = false;
            m_wallContactTime = 0f;
        }
        
        // ПЕРЕХОД В АНИМАЦИЮ ПАДЕНИЯ ПРИ ДВИЖЕНИИ ОТ СТЕНЫ
        if (m_wasWallSliding && !m_isWallSliding && !m_grounded)
        {
            m_animator.SetTrigger("Jump");
        }
        
        m_animator.SetBool("WallSlide", m_isWallSliding);

        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack = m_currentAttack > 2 ? 1 : m_currentAttack + 1;
            if (m_timeSinceAttack > 1.0f) m_currentAttack = 1;
            m_animator.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;
        }
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
        else if (Input.GetKeyDown("space"))
        {
            if (m_grounded && !m_rolling)
                PerformJump();
            else if (m_isWallSliding && !m_rolling)
                PerformWallJump();
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if(m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    void CreateWallSlideDust()
    {
        if (m_slideDust == null) return;
        
        Vector3 spawnPosition = m_facingDirection == 1 ? 
            m_wallSensorR2.transform.position : 
            m_wallSensorL2.transform.position;
        
        GameObject dust = Instantiate(m_slideDust, spawnPosition, Quaternion.identity);
        dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        Destroy(dust, 1.0f);
    }

    void PerformJump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_groundSensor.Disable(0.2f);
    }
    
    void PerformWallJump()
    {
        m_animator.SetTrigger("Jump");
        
        // Определяем сторону стены в момент прыжка
        bool isTouchingWallRight = (m_wallSensorR1.State() && m_wallSensorR2.State());
        bool isTouchingWallLeft = (m_wallSensorL1.State() && m_wallSensorL2.State());
        
        int jumpDirection = 0;
        
        if (isTouchingWallRight)
        {
            jumpDirection = -1; // От правой стены - влево
        }
        else if (isTouchingWallLeft)
        {
            jumpDirection = 1; // От левой стены - вправо
        }
        
        if (jumpDirection == 0)
        {
            jumpDirection = -m_facingDirection;
        }
        
        float wallJumpHorizontalForce = 8.0f;
        float wallJumpVerticalForce = 9.0f;
        
        m_body2d.velocity = new Vector2(jumpDirection * wallJumpHorizontalForce, wallJumpVerticalForce);
        
        GetComponent<SpriteRenderer>().flipX = jumpDirection < 0;
        m_facingDirection = jumpDirection;
        
        m_isWallSliding = false;
        m_wallContactTime = 0f;
        m_animator.SetBool("WallSlide", false);
        
        CreateWallSlideDust();
        m_groundSensor.Disable(0.2f);
    }

    void AE_SlideDust()
    {
        if (m_slideDust == null) return;
        
        Vector3 spawnPosition = m_facingDirection == 1 ? 
            m_wallSensorR2.transform.position : 
            m_wallSensorL2.transform.position;
        
        GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation);
        dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        Destroy(dust, 1.0f);
    }
}