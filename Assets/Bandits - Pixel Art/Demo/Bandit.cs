using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    public float                m_speed = 3f;
    public float                m_jumpForce = 7.5f;
    public float                inputX = 1;
    public float                m_attackRange = 1.5f;
    public float                m_agroDistance = 5f;
    public float                m_jumpDistance = 5f;
    public float m_attackArea;

    public int hp = 10;
    public int damage = 2;
    public int coins = 3;
    public Vector3 attackOffset;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    public  Sensor_Bandit       m_groundSensor;
    public Transform player;

    public bool                m_grounded = false;
    public bool                m_combatIdle = false;
    private bool                m_isDead = false;
    public bool                 m_isJump = false;
    public bool                 m_canJump = true;

    public GameObject coin;
    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_isJump= false;
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State() && !m_isJump) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        else if (m_grounded && !m_groundSensor.State() && m_isJump)
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (!m_isJump && m_grounded)
        {
            if (player.position.x < transform.position.x)
            {
                inputX = -1;
            }
            else
            {
                inputX = 1;
            }
        }

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

    }

    public void StartCor()
    {
        StartCoroutine(JumpCoolDown());
    }
    public IEnumerator JumpCoolDown()
    {
        yield return new WaitForSeconds(10f);
        m_canJump= true;
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x*-inputX;
        pos += transform.up * attackOffset.y;
        Collider2D player = Physics2D.OverlapCircle(pos, m_attackArea);
        
        if (player != null && player.GetComponent<HeroKnight>())
        {
            player.GetComponent<HeroKnight>().GetDamage(damage);
        }
    }

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
        hp -= damage;
        m_animator.SetTrigger("Hurt");

        if (hp <= 0)
        {
            Drop();
            gameObject.tag = "Corsp";
            m_animator.SetInteger("AnimState", 0);
            m_animator.SetTrigger("Death");
        }
        else if (hp>0){
            m_animator.SetInteger("AnimState", 1);
        }

    }

    public void Drop()
    {

        for (int i = 0; i < coins; i++)
        {
            GameObject coin_obj = Instantiate(coin, this.transform);
            coin_obj.transform.parent = null;
           
        }
    }
}
