using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Windows;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    public Sensor_Bandit m_groundSensor;
    public Transform player;
    public GameObject bullet;
    public GameObject spawn;

    public bool m_grounded = false;
    public bool m_isJump = false;
    public bool is_active = false;
    private bool DEAD = false;

    public float m_speed = 3f;
    public float m_jumpForce = 7.5f;
    public float inputX = -1;
    public float m_attackArea;
    public float m_agroDistance = 10f;

    public int hp = 10;
    public int damage = 2;


    public Vector3 attackOffset;
    void Start()
    {
        m_groundSensor = GetComponentInChildren<Sensor_Bandit>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("IsGrounded", m_grounded);
            m_isJump = false;
        }

        if (m_grounded && !m_groundSensor.State() && !m_isJump)
        {
            m_grounded = false;
            m_animator.SetBool("IsGrounded", m_grounded);
        }
        else if (m_grounded && !m_groundSensor.State() && m_isJump)
        {
            m_grounded = false;
            m_animator.SetBool("IsGrounded", m_grounded);
        }

        //if (!m_isJump && m_grounded)
        //{
        //    if (player.position.x < transform.position.x)
        //    {
        //        inputX = -1;
        //    }
        //    else
        //    {
        //        inputX = 1;
        //    }
        //}

        //if (inputX > 0)
        //    transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
        //else if (inputX < 0)
        //    transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);

        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
        m_animator.SetBool("IsGrounded", m_grounded);
        if (Vector2.Distance(player.position, m_body2d.position) < m_agroDistance && is_active)
        {
            m_animator.SetTrigger("Idle");
            is_active= false;
        }

        if (DEAD && m_grounded)
        {
            m_body2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            m_animator.SetTrigger("Death");
        }
    }


    public void Attack()
    {
        GameObject obj = Instantiate(bullet, spawn.transform);
        obj.GetComponent<bullet>().boss = this.GetComponent<Boss>();
        obj.transform.parent = null;
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    
    public void GetDamage(int damage)
    {
        hp -= damage;
        //m_animator.SetTrigger("Hurt");
        GetComponent<SpriteRenderer>().color= Color.black;
        StartCoroutine(attack_effect());

        if (hp <= 0)
        {   gameObject.tag = "Corsp";
            DEAD = true;
           
            /*this.GetComponent<Boss>().enabled =false;*/
            
        }
        else if (hp > 0)
        {
           
            m_animator.SetTrigger("Idle");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_body2d.velocity = new Vector2(0, 0);
    }

    IEnumerator attack_effect()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
